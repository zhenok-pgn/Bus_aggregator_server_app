namespace App.Core.Entities
{
    public class UtcTimezone
    {
        public required string Name { get; set; }
        public int OffsetMinutes { get; set; }
    }
}
