namespace App.Core.Entities
{
    //определяет маршрут поездки
    public class Route
    {
        /*public int Id { get; set; }
        public string? Name { get; set; }
        public Carrier? Carrier { get; set; }
        public List<RoutePoint> RoutePoints { get; set; } = new();
        public List<RouteSchedule> RouteSchedules { get; set; } = new();*/

        /*вторая идея
         * 
         * public int Id { get; set; }
        public string? Name { get; set; }
        public Carrier? Carrier { get; set; }
        public RoutePoint From {get; set;}
        public RoutePoint To {get; set;}
        public int? ParentRouteId { get; set;}
        public Route? ParentRoute {get; set;}*/

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Number { get; set; }
        public int CarrierId { get; set; }
        public Carrier? Carrier { get; set; }
        public List<RouteStop> RouteStops { get; set; } = new();
        public List<RouteSchedule> RouteSchedules { get; set; } = new();
        public List<Tariff> Tariffs { get; set; } = new();
    }
}
