namespace App.Core.Entities
{
    public class Locality
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Okato { get; set; }
        public int UtcTimezoneId { get; set; }
        public UtcTimezone? UtcTimezone { get; set; }
    }
}
