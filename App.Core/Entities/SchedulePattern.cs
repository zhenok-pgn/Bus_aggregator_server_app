using App.Core.Enums;

namespace App.Core.Entities
{
    public class SchedulePattern
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DaysOfWeekFlags DaysOfWeek { get; set; }
    }
}
