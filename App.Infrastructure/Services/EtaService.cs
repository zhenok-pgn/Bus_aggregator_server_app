using App.Application.DTO;
using App.Application.Services;
using App.Core.Enums;
using App.Core.Helpers;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace App.Infrastructure.Services
{
    public class EtaService : IEtaService
    {
        private readonly ApplicationDBContext _db;
        private readonly OsrmService _osrmService;
        private readonly IMapper _mapper;
        private readonly IRouteSegmentScheduleService _routeSegmentScheduleService;
        private readonly IBusLocationService _busLocationService;
        private readonly ITripNotifier _tripNotifier;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _etaCacheTtl = TimeSpan.FromMinutes(60);
        private readonly TimeSpan _requestCounterCacheTtl = TimeSpan.FromMinutes(30);

        public EtaService
            (ApplicationDBContext db, 
            IMapper mapper, 
            IRouteSegmentScheduleService routeSegmentScheduleService, 
            IBusLocationService busLocationService,
            ITripNotifier tripNotifier,
            OsrmService osrmService,
            IMemoryCache memoryCache)
        {
            _db = db;
            _mapper = mapper;
            _routeSegmentScheduleService = routeSegmentScheduleService;
            _busLocationService = busLocationService;
            _osrmService = osrmService;
            _cache = memoryCache;
            _tripNotifier = tripNotifier;
        }

        public async Task<TripEtaDTO?> CalculateEtaAsync(int tripId)
        {
            var tripStatus = await _db.Trips
                .Where(t => t.Id == tripId)
                .Select(t => t.TripStatus)
                .FirstOrDefaultAsync();
            if (tripStatus != TripStatus.InProgress)
                return null; // ЭТА не рассчитывается для рейсов, которые еще не начались или уже завершены

            var cacheKey = $"ETA_{tripId}";
            var counterKey = $"ETA_COUNTER_{tripId}";

            var requestCount = _cache.GetOrCreate(counterKey, entry =>
            {
                entry.SlidingExpiration = _requestCounterCacheTtl; // Сброс счётчика через время
                return 0;
            });

            requestCount++;

            _cache.Set(counterKey, requestCount, _requestCounterCacheTtl);

            if (_cache.TryGetValue(cacheKey, out TripEtaDTO cachedEta) && requestCount % 4 != 0)
            {
                return cachedEta;
            }

            var tripEta = await CalculateEtaInternalAsync(tripId);
            _cache.Set(cacheKey, tripEta, _etaCacheTtl);

            // Запись объекта в файл JSON
            var filePath = Path.Combine(AppContext.BaseDirectory, $"ETA_{tripId}.log");
            var json = JsonSerializer.Serialize(tripEta);
            await File.AppendAllTextAsync(filePath, json + Environment.NewLine);

            await _tripNotifier.SendEtaUpdateAsync(tripId, tripEta);
            return tripEta;
        }

        private async Task<TripEtaDTO> CalculateEtaInternalAsync(int tripId)
        {
            var segmentIds = await _routeSegmentScheduleService.GetSegmentsFromFirstStop(tripId); // routesegmentId
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
            var remainingIds = segmentIds
                .Where(id => !completedSegments.Any(s => s.RouteSegmentId == id))
                .ToList(); // Фильтрация с сохранением порядка
            var remainingSegments = await _db.TripExecutions
                .Where(te => te.TripId == tripId && remainingIds.Contains(te.RouteSegmentId))
                 .Include(te => te.RouteSegment)
                 .ThenInclude(rs => rs.ToStop)
                        .ThenInclude(s => s.Locality)
                            .ThenInclude(l => l.UtcTimezone)
                .ToListAsync();
            remainingSegments = remainingSegments
                .OrderBy(te => remainingIds.IndexOf(te.RouteSegmentId))
                .ToList();

            var curSpeed = await _busLocationService.GetBusAverageSpeedAsync(tripId);
            var busCoords = await _busLocationService.GetLatestBusLocationAsync(tripId);
            var tripEta = new TripEtaDTO
            {
                TripId = tripId.ToString(),
                CurrentTime = busCoords.Timestamp, //DateTimeOffset.UtcNow,
                StopEtas = new List<StopEtaDTO>() {
                    new StopEtaDTO {
                        StopId = firstStop.RouteSegment.FromStopId.ToString(),
                        StopName = firstStop.RouteSegment.FromStop.Name,
                        Latitude = firstStop.RouteSegment.FromStop.Latitude,
                        Longitude = firstStop.RouteSegment.FromStop.Longitude,
                    } }
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
                };
                tripEta.StopEtas.Add(stopEta);
            }

            // Среднее время прохождения непройденных сегментов
            var tHistDurationsInSeconds = await GetAvarageDurations(remainingSegments.Select(s => s.RouteSegmentId).ToList());
            for (int i = 0; i < remainingSegments.Count; i++)
            {
                double? totalDistance = null;
                double? remainingDistance = null;
                if (curSpeed != null)
                {
                    var coords = new List<(double, double)> {
                        (firstStop.RouteSegment.FromStop.Latitude, firstStop.RouteSegment.FromStop.Longitude)};
                    remainingDistance = GpsValidator.Haversine(busCoords.Latitude, busCoords.Longitude,
                        remainingSegments[i].RouteSegment.ToStop.Latitude, remainingSegments[i].RouteSegment.ToStop.Longitude);
                    for (int j = 0; j <= i; j++)
                    {
                        coords.Add((remainingSegments[j].RouteSegment.ToStop.Latitude, remainingSegments[j].RouteSegment.ToStop.Longitude));
                    }
                    totalDistance = await _osrmService.GetDistanceInMetersAsync(coords) ?? throw new InvalidOperationException("Ошибка расчета расстояния");
                }

                var tEstimatedSeconds = EtaEstimator.EstimateEtaSeconds(
                    totalDistance ?? -1, 
                    remainingDistance ?? -1,
                    curSpeed ?? 0,
                    tHistDurationsInSeconds[remainingSegments[i].RouteSegmentId], 
                    (busCoords.Timestamp - remainingSegments[0].Departure).Value.TotalSeconds);

                var stopEta = new StopEtaDTO
                {
                    StopId = remainingSegments[i].RouteSegment.ToStop.Id.ToString(),
                    StopName = remainingSegments[i].RouteSegment.ToStop.Name,
                    Latitude = remainingSegments[i].RouteSegment.ToStop.Latitude,
                    Longitude = remainingSegments[i].RouteSegment.ToStop.Longitude,
                    TimezoneOffset = remainingSegments[i].RouteSegment.ToStop.Locality.UtcTimezone.OffsetMinutes,
                    EstimatedArrival = tEstimatedSeconds == null ? null : firstStop.Departure.Value.AddSeconds(tEstimatedSeconds.Value),
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

            /*var averageDurations = rawPassages
                .GroupBy(sp => sp.RouteSegmentId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Take(20) // последние 20 прохождений сегмента
                         .Average(sp => (sp.Arrival! - sp.Departure!).Value.TotalSeconds
                    )
                );*/

            var averageDurations = routeSegmentIds.ToDictionary(
                id => id,
                id =>
                {
                    var recent = rawPassages
                        .Where(sp => sp.RouteSegmentId == id)
                        .Take(20)
                        .ToList();

                    if (recent.Count == 0)
                    {
                        return 0; // в случае отсутствия истории
                    }

                    return recent.Average(sp => (sp.Arrival!.Value - sp.Departure!.Value).TotalSeconds);
                }
            );

            return averageDurations;
        }
    }
}
