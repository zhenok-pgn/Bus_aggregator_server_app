namespace App.Core.Entities
{
    public class BusLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public int TripId { get; set; }
        public Trip? Trip { get; set; }
    }
}
