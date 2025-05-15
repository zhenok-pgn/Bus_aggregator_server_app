namespace App.Core.Entities
{
    public class RouteSegment
    {
        public int Id { get; set; }
        public int FromStopId { get; set; }
        public BusStop? FromStop { get; set; }
        public int ToStopId { get; set; }
        public BusStop? ToStop { get; set; }
    }
}
