namespace App.Core.Entities
{
    public class UtcTimezone
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int OffsetMinutes { get; set; }
    }
}
