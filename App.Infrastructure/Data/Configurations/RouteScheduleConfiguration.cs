using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
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
