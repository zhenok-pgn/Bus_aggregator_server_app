namespace App.Core.Entities
{
    public class TripExecution
    {
        public int TripId { get; set; }
        public int RouteSegmentId { get; set; }
        public RouteSegment? RouteSegment { get; set; }
        public DateTimeOffset? Departure {  get; set; }
        public DateTimeOffset? Arrival { get; set; }
    }
}
