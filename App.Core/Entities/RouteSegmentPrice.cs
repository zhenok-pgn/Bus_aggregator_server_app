namespace App.Core.Entities
{
    /// <summary>
    /// Каждый сегмент маршрута имеет цену в зависимости от тарифа
    /// </summary>
    public class RouteSegmentPrice
    {
        public int Id { get; set; }
        public int TariffId { get; set; }
        public Tariff? Tariff { get; set; }
        public int RouteStopFromId { get; set; }
        public RouteStop? RouteStopFrom { get; set; }
        public int RouteStopToId { get; set; }
        public RouteStop? RouteStopTo { get; set; }
        public int Price { get; set; }
    }
}
