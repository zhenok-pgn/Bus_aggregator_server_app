using App.Core.Entities;

namespace App.Application.DTO
{
    public class RouteStopDTO
    {
        public int Id { get; set; }
        public int BusStopId { get; set; }
        public string? BusStopName { get; set; }
        public StationType BusStopType { get; set; }
        public string? BusStopAddress { get; set; }
        public int StopTimeInMinutes { get; set; } // время стоянки на остановке
        public int MinutesFromStart { get; set; } // время с начала маршрута
        public int DistanceFromStart { get; set; }
        public int Order { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
