using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class SchedulePatternConfiguration : IEntityTypeConfiguration<SchedulePattern>
    {
        public void Configure(EntityTypeBuilder<SchedulePattern> builder)
        {
            builder.HasIndex(sp => new { sp.StartDate, sp.EndDate, sp.DaysOfWeek })
            .IsUnique();
            builder.ToTable(t => t.HasCheckConstraint("CHK_SchedulePattern_EndDateAfterStartDate",
            @"""EndDate"" >= ""StartDate"""));
        }
    }
}
