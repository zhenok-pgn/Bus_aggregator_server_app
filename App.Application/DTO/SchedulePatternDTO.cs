using App.Core.Enums;

namespace App.Application.DTO
{
    public class SchedulePatternDTO
    {
        public required string Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int DaysOfWeek { get; set; }
    }
}