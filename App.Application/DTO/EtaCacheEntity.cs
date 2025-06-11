namespace App.Application.DTO
{
    public class EtaCacheEntry
    {
        public DateTimeOffset LastUpdatedUtc { get; set; }
        public TripEtaDTO TripEta { get; set; } = default!;
    }
}
