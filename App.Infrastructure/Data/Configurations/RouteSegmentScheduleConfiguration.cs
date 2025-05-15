using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class RouteSegmentScheduleConfiguration : IEntityTypeConfiguration<RouteSegmentSchedule>
    {
        public void Configure(EntityTypeBuilder<RouteSegmentSchedule> builder)
        {
            //builder.Property(p => p.Id)
            //    .HasColumnType("bigint"); // сделать bigint
            builder.HasIndex(rss => new { rss.RouteSegmentId, rss.RouteScheduleId, rss.Version }).IsUnique();
            builder.Property(rss => rss.Price).HasPrecision(10, 2);
            builder.ToTable(t => {
                t.HasCheckConstraint("CK_Arrival_Day_Number_Range", @"""ArrivalDayNumber"" >= 0 AND ""ArrivalDayNumber"" <= 30");
                t.HasCheckConstraint("CK_Version_Positive", @"""Version"" > 0");
                t.HasCheckConstraint("CK_Arrival_Vs_Departure", @"""ArrivalDayNumber"" > 0 OR ""DepartureTime"" >= ""ArrivalTime""");
                t.HasCheckConstraint("CK_Price_Non_Negative", @"""Price"" >= 0");
            });
        }
    }
}
