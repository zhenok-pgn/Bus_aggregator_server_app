namespace App.Core.Entities
{
    /// <summary>
    /// Связывает Route и BusStop многие ко многим. Определяет точки на маршруте в порядке следования
    /// </summary>
    public class RouteStop
    {
        /*public int Id { get; set; }
        public int StationId { get; set; }
        public Station? Station { get; set; }
        public int RouteId { get; set; }
        public Route? Route { get; set; }
        public int? RoutePointId { get; set; }
        public RoutePoint? PreviousRoutePoint { get; set; }
        public bool IsBoarding { get; set; }
        public bool IsDisembarkation { get; set; }
        public bool IsLongTermParking { get; set; }
        public TimeOnly? HoursOnTheRoad { get; set; }
        public TimeOnly? HoursOfTheParking { get; set; }*/

        public int Id { get; set; }
        public int Order { get; set; } // порядок следования по маршруту
        public int RouteId { get; set; }
        public Route? Route { get; set; }
        public int BusStopId { get; set; }
        public BusStop? BusStop { get; set; }
        public int StopTimeInMinutes { get; set; } // время стоянки на остановке
        public int MinutesFromStart { get; set; } // время с начала маршрута
        public int DistanceFromStart { get; set; } // расстояние с начала маршрута
    }
}
