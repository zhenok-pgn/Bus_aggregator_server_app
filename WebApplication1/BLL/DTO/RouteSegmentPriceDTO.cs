using App.DAL.Entities;

namespace App.WEB.BLL.DTO
{
    public class RouteSegmentPriceDTO
    {
        public int Id { get; set; }
        public required RouteStopDTO RouteStopFrom { get; set; }
        public required RouteStopDTO RouteStopTo { get; set; }
        public int Price { get; set; }
    }
}
