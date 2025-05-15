namespace App.Core.Entities
{
    public class RouteSchedule
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public Route? Route { get; set; }
        public int TariffId { get; set; }
        public Tariff? Tariff { get; set; }
        public int SchedulePatternId { get; set; }
        public SchedulePattern? SchedulePattern { get; set; }
        public TimeOnly DepartureTime { get; set; }
    }
}
