using App.DAL.Entities;

namespace App.WEB.BLL.DTO
{
    public class RouteScheduleDTO
    {
        public int Id { get; set; }
        public required TariffDTO Tariff { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Periodicity Periodicity { get; set; }
        public string? DaysOfWeek { get; set; } // if Periodicity is 'ByDaysOfTheWeek'
        public DateOnly? StartWith { get; set; } // if Periodicity is 'ByNumbers'
        public int? Interval { get; set; } // if Periodicity is 'ByNumbers'
        public string? DepartureTimes { get; set; }
        public SeatingType SeatingType { get; set; }
        public required string BaseSeatingPlan { get; set; }
    }
}
