namespace App.Application.DTO
{
    public class LocalityDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Region { get; set; }
        public required string Country { get; set; }
        public required string District { get; set; }
        public required string Timezone { get; set; }
        public int OffsetMinutes { get; set; }
    }
}