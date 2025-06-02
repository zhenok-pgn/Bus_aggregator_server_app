namespace App.Application.DTO
{
    public class RouteScheduleSummaryDTO
    {
        public required string Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int DaysOfWeek { get; set; }
        public TimeOnly DepartureTime {  get; set; }
        public required RouteSummaryDTO Route { get; set; }
    }
}
