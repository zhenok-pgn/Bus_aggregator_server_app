namespace App.Application.DTO
{
    public class RouteDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string RegistrationNumber { get; set; } // регистрационный номер
        public required string Number { get; set; }
        public int CarrierId { get; set; }
        public required List<RouteScheduleDTO> RouteSchedules { get; set; }
    }
}
