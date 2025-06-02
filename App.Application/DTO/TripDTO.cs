namespace App.Application.DTO
{
    public class TripDTO
    {
        public required string Id { get; set; }
        public required RouteSummaryDTO Route { get; set; }
        public required BusDTO Bus { get; set; }
        public required DriverDTO Driver { get; set; }
        public required RouteSegmentScheduleDTO Schedule { get; set; }
        public DateOnly DepartureDate { get; set; }
        public required string TripStatus { get; set; }
        public List<SeatDTO>? Seats { get; set; } 
    }
}
