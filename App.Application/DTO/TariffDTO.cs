namespace App.Application.DTO
{
    public class TariffDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Currency { get; set; }
        public required List<RouteSegmentPriceDTO> Prices { get; set; }
    }
}
