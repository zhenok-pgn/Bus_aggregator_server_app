namespace App.Core.Entities
{
    public class Locality
    {
        public int OsmId { get; set; }
        public required string Name { get; set; }
        public required string Region { get; set; }
        public required string Country { get; set; }
        public required string District { get; set; }
        public required string UtcTimezoneName { get; set; }
        public UtcTimezone? UtcTimezone { get; set; }
    }
}
