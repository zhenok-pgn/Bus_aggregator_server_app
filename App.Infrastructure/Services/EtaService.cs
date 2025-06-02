using App.Application.DTO;
using App.Application.Services;
using App.Core.Helpers;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Services
{
    public class EtaService : IEtaService
    {
        private readonly ApplicationDBContext _db;
        private readonly OsrmService _osrmService;
        private readonly IMapper _mapper;
        private readonly IRouteSegmentScheduleService _routeSegmentScheduleService;
        private readonly IBusLocationService _busLocationService;

        public EtaService(ApplicationDBContext db, IMapper mapper, IRouteSegmentScheduleService routeSegmentScheduleService, IBusLocationService busLocationService, OsrmService osrmService)
        {
            _db = db;
            _mapper = mapper;
            _routeSegmentScheduleService = routeSegmentScheduleService;
            _busLocationService = busLocationService;
            _osrmService = osrmService;
        }

        public async Task<TripEtaDTO> CalculateEtaAsync(int tripId)
        {
            var segmentIds = await _routeSegmentScheduleService.GetSegmentsFromFirstStop(tripId);
            var firstStop = await _db.TripExecutions
                .Where(te => te.TripId == tripId && te.RouteSegmentId == segmentIds.First())
                .Include(rs => rs.RouteSegment)
                    .ThenInclude(rs => rs.FromStop)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException("Рейс еще не начался");
            var completedSegments = await _db.TripExecutions
                .Where(te => te.TripId == tripId && segmentIds.Contains(te.RouteSegmentId) && te.Arrival != null)
                 .Include(te => te.RouteSegment)
                 .ThenInclude(rs => rs.ToStop)
                        .ThenInclude(s => s.Locality)
                            .ThenInclude(l => l.UtcTimezone)
                .OrderBy(te => te.Arrival)
                .ToListAsync();

            // Исключаем completedSegments из segmentIds
            var remainingSegment = await _db.TripExecutions
                .Where(te => te.TripId == tripId && !completedSegments.Select(s => s.RouteSegmentId).Contains(te.RouteSegmentId))
                 .Include(te => te.RouteSegment)
                 .ThenInclude(rs => rs.ToStop)
                        .ThenInclude(s => s.Locality)
                            .ThenInclude(l => l.UtcTimezone)
                .OrderBy(te => te.Arrival)
                .ToListAsync(); segmentIds.Except(completedSegments.Select(s => s.RouteSegmentId)).ToList();

            
            var tripEta = new TripEtaDTO
            {
                TripId = tripId.ToString(),
                CurrentTime = DateTimeOffset.UtcNow,
                StopEtas = new List<StopEtaDTO>()
            };
            foreach (var s in completedSegments)
            {
                var stopEta = new StopEtaDTO
                {
                    StopId = s.RouteSegment.ToStop.Id.ToString(),
                    StopName = s.RouteSegment.ToStop.Name,
                    Latitude = s.RouteSegment.ToStop.Latitude,
                    Longitude = s.RouteSegment.ToStop.Longitude,
                    TimezoneOffset = s.RouteSegment.ToStop.Locality.UtcTimezone.OffsetMinutes,
                    EstimatedArrival = s.Arrival!.Value,
                    Delay = null // Здесь можно добавить логику для расчета задержки, если нужно
                };
                tripEta.StopEtas.Add(stopEta);
            }

            var curSpeed = await _busLocationService.GetBusAverageSpeedAsync(tripId);
            var curBusLocation = await _busLocationService.GetLatestBusLocationAsync(tripId);
            // Среднее время прохождения непройденных сегментов
            var tHistDurationsInSeconds = await GetAvarageDurations(remainingSegment.Select(s => s.RouteSegmentId).ToList());
            for(int i = 0; i < remainingSegment.Count; i++)
            {
                var coords = new List<(double, double)>
                {
                    (firstStop.RouteSegment.FromStop.Latitude, firstStop.RouteSegment.FromStop.Longitude)
                };
                for(int j = 0; j <= i; j++)
                {
                    coords.Add((remainingSegment[j].RouteSegment.ToStop.Latitude, remainingSegment[j].RouteSegment.ToStop.Longitude));
                }
                var totalDistance = await _osrmService.GetDistanceInMetersAsync(coords);
                var tCurrentSeconds = totalDistance / curSpeed ?? throw new InvalidOperationException("Ошибка расчета времени в пути");
                var tEstimatedSeconds = EtaEstimator
                    .EstimateEtaSeconds(tCurrentSeconds, tHistDurationsInSeconds[remainingSegment[i].RouteSegmentId]);

                var stopEta = new StopEtaDTO
                {
                    StopId = remainingSegment[i].RouteSegment.ToStop.Id.ToString(),
                    StopName = remainingSegment[i].RouteSegment.ToStop.Name,
                    Latitude = remainingSegment[i].RouteSegment.ToStop.Latitude,
                    Longitude = remainingSegment[i].RouteSegment.ToStop.Longitude,
                    TimezoneOffset = remainingSegment[i].RouteSegment.ToStop.Locality.UtcTimezone.OffsetMinutes,
                    EstimatedArrival = firstStop.Arrival.Value.AddSeconds(tEstimatedSeconds),
                    Delay = null // Здесь можно добавить логику для расчета задержки, если нужно
                };
                tripEta.StopEtas.Add(stopEta);
            }
            
            return tripEta;
        }

        private async Task<Dictionary<int, double>> GetAvarageDurations(List<int> routeSegmentIds)
        {
            var rawPassages = await _db.TripExecutions
                .Where(sp => routeSegmentIds.Contains(sp.RouteSegmentId))
                .Where(sp => sp.Arrival != null && sp.Departure != null) // Только завершенные сегменты
                .OrderByDescending(sp => sp.Departure) // Сначала новые
                .ToListAsync();

            var averageDurations = rawPassages
                .GroupBy(sp => sp.RouteSegmentId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Take(20) // последние 20 прохождений сегмента
                         .Average(sp => (sp.Arrival! - sp.Departure!).Value.TotalSeconds
                    )
                );

            return averageDurations;
        }
    }
}
