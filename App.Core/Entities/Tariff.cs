namespace App.Core.Entities
{
    public enum Currency
    {
        USD,
        EUR,
        RUB,
        KZT
    }

    public class Tariff
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Currency Currency { get; set; }
        public int RouteId { get; set; }
        public Route? Route { get; set; }
        public List<RouteSegmentPrice> Prices { get; set; } = new();
    }
}
