namespace App.Core.Entities
{
    public class RouteSegmentSchedule
    {
        public int Id { get; set; }
        public required string SegmentNumber { get; set; }
        public int RouteSegmentId { get; set; }
        public RouteSegment? RouteSegment { get; set; }
        public int RouteScheduleId { get; set; }
        public RouteSchedule? RouteSchedule { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public int ArrivalDayNumber { get; set; }
        public decimal Price { get; set; }
        public int Version { get; set; }
    }
}
