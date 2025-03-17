using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Entities
{
    public enum Periodicity
    {
        Daily,
        ByDaysOfTheWeek,
        ByNumbers
    }

    public enum DaysOfWeek
    {
        Mon,
        Tue,
        Wed,
        Thu,
        Fri,
        Sat,
        Sun
    }

    public enum SeatingType
    {
        Free,
        Choice
    }

    public class RouteSchedule
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public Route? Route { get; set; }
        public int TariffId { get; set; }
        public Tariff? Tariff { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Periodicity Periodicity { get; set; }
        public string? DaysOfWeek { get; set; } // if Periodicity is 'ByDaysOfTheWeek'
        public DateOnly? StartWith {  get; set; } // if Periodicity is 'ByNumbers'
        public int? Interval { get; set; } // if Periodicity is 'ByNumbers'
        public string? DepartureTimes { get; set; }
        public SeatingType SeatingType { get; set; }
        public required string BaseSeatingPlan { get; set; }
    }

    public class RouteScheduleConfiguration : IEntityTypeConfiguration<RouteSchedule>
    {
        public void Configure(EntityTypeBuilder<RouteSchedule> builder)
        {
            builder.Property(p => p.DepartureTimes)
                .IsRequired();
            builder.Property(p => p.BaseSeatingPlan)
                .IsRequired();
        }
    }
}
