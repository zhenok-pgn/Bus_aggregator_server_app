using App.Core.Entities;

namespace App.Application.DTO
{
    public class RouteSegmentScheduleDTO
    {
        public required string Id { get; set; }
        public required string SegmentNumber { get; set; }
        public required RouteSegmentDTO RouteSegment { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public int ArrivalDayNumber { get; set; }
        public int? DepartureDayNumber { get; set; }
        public decimal Price { get; set; }
    }
}
