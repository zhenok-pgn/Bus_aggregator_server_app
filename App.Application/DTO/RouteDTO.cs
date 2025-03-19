namespace App.Application.DTO
{
    public class RouteDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Number { get; set; }
        public required CarrierDTO Carrier { get; set; }
        public int TotalDuration { get; set; }
        public int TotalDistance { get; set; }
        public required List<RouteStopDTO> RouteStops { get; set; }
        public required List<TariffDTO> Tariffs { get; set; }
        public required List<RouteScheduleDTO> RouteSchedules { get; set; }
    }
}
