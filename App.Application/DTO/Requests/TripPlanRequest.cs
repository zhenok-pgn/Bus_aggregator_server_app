namespace App.Application.DTO.Requests
{
    public class TripPlanRequest
    {
        public int RouteScheduleId { get; set; }
        public int BusId { get; set; }
        public int DriverId { get; set; }
        public DateOnly FromPlanning { get; set; }
        public DateOnly ToPlanning { get; set; }
    }
}
