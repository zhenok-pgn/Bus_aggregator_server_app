namespace App.Core.Entities
{
    //определяет маршрут поездки
    public class Route
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string RegistrationNumber { get; set; } // регистрационный номер
        public required string Number { get; set; }
        public int CarrierId { get; set; }
        public Carrier? Carrier { get; set; }
        public List<RouteSchedule> RouteSchedules { get; set; } = new();
    }
}
