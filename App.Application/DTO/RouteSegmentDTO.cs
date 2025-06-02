namespace App.Application.DTO
{
    public class RouteSegmentDTO
    {
        public required string Id { get; set; }
        public required BusStopDTO From { get; set; }
        public required BusStopDTO To { get; set; }
    }
}