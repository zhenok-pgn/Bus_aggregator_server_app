using App.Application.DTO;
using App.Application.Services;
using App.Core.Entities;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Infrastructure.Services
{
    public  class RouteSegmentScheduleService :IRouteSegmentScheduleService
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;

        public RouteSegmentScheduleService(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<RouteSegmentScheduleDTO>> GetSequentialSegments(int tripId)
        {
            var trip = await _db.Trips.FirstOrDefaultAsync(t => t.Id == tripId)
                ?? throw new KeyNotFoundException("Trip not found");

            var allSegments = await _db.RouteSegmentSchedules
                .Where(rss => rss.RouteScheduleId == trip.RouteScheduleId)
                .Include(rss => rss.RouteSegment)
                    .ThenInclude(rs => rs.FromStop)
                        .ThenInclude(s => s.Locality)
                .Include(rss => rss.RouteSegment)
                    .ThenInclude(rs => rs.ToStop)
                        .ThenInclude(s => s.Locality)
                .ToListAsync();

            // Фильтруем только базовые последовательные сегменты (X-Y, где Y = X+1)
            var basicSegments = allSegments
                .Where(s => {
                    var parts = s.SegmentNumber.Split('-');
                    if (parts.Length != 2) return false;
                    return int.Parse(parts[1]) == int.Parse(parts[0]) + 1;
                })
                .OrderBy(s => int.Parse(s.SegmentNumber.Split('-')[0]))
                .ToList();

            // Проверяем, что сегменты образуют непрерывную цепочку
            for (int i = 0; i < basicSegments.Count - 1; i++)
            {
                var currentEnd = int.Parse(basicSegments[i].SegmentNumber.Split('-')[1]);
                var nextStart = int.Parse(basicSegments[i + 1].SegmentNumber.Split('-')[0]);

                if (currentEnd != nextStart)
                {
                    throw new InvalidOperationException(
                        $"Segments are not sequential. Segment {basicSegments[i].SegmentNumber} " +
                        $"does not connect to {basicSegments[i + 1].SegmentNumber}");
                }
            }

            // Преобразуем в DTO
            return _mapper.Map<List<RouteSegmentScheduleDTO>>(basicSegments);
        }

        public async Task<List<int>> GetSegmentsFromFirstStop(int tripId)
        {
            var trip = await _db.Trips.FirstOrDefaultAsync(t => t.Id == tripId)
                ?? throw new KeyNotFoundException("Trip not found");

            var segments = await _db.RouteSegmentSchedules
                .Where(rss => rss.RouteScheduleId == trip.RouteScheduleId && rss.SegmentNumber.StartsWith("1-"))
                .ToListAsync();

            var ordered = segments
                .OrderBy(rss => int.Parse(rss.SegmentNumber.Split('-')[1]))
                .Select(rss => rss.Id)
                .ToList();

            return ordered;
        }

        /// <summary>
        /// ищет все родительские и дочерние сегменты целевого сегмента 
        /// </summary>
        /// <param name="targetSegment"></param>
        /// <returns></returns>
        public async Task<List<int>> GetRelatedSegments(RouteSegmentSchedule targetSegment)
        {
            var parts = targetSegment.SegmentNumber.Split('-');
            int start = int.Parse(parts[0]);
            int end = int.Parse(parts[1]);

            var filteredSegments = await _db.RouteSegmentSchedules
                .Where(rs => rs.RouteScheduleId == targetSegment.RouteScheduleId)
                .Select(rs => new { rs.Id, rs.SegmentNumber })
                .ToListAsync();

            // 1. Включающие сегменты (родительские)
            var includingSegments = filteredSegments
                .Where(rs => {
                    var segParts = rs.SegmentNumber.Split('-');
                    return int.Parse(segParts[0]) <= start && int.Parse(segParts[1]) >= end;
                })
                .Select(rs => rs.Id)
                .ToList();

            // 2. Вложенные сегменты (дочерние)
            var includedSegments = filteredSegments
                .Where(rs => {
                    var segParts = rs.SegmentNumber.Split('-');
                    return int.Parse(segParts[0]) >= start && int.Parse(segParts[1]) <= end;
                })
                .Select(rs => rs.Id)
                .ToList();

            var allRelatedSegments = includingSegments.Concat(includedSegments).Distinct().ToList();
            return allRelatedSegments;
        }

        public async Task<int> GetDepartureDayNumber(RouteSegmentSchedule targetSegment)
        {
            var parts = targetSegment.SegmentNumber.Split('-');
            int targetStart = int.Parse(parts[0]);

            var previousSegmentArrivalDayNumber = await _db.RouteSegmentSchedules
                .Where(rs => rs.SegmentNumber.EndsWith($"_{targetStart}"))
                .OrderBy(rs => rs.SegmentNumber)
                .Select(rs => rs.ArrivalDayNumber)
                .FirstOrDefaultAsync();

            return previousSegmentArrivalDayNumber;
        }
    }
}
