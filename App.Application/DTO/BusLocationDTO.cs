namespace App.Application.DTO
{
    public class BusLocationDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public required string TripId { get; set; }
    }
}
