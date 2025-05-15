using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class TripExecutionConfiguration : IEntityTypeConfiguration<TripExecution>
    {
        public void Configure(EntityTypeBuilder<TripExecution> builder)
        {
            builder.HasKey(t => new { t.TripId, t.RouteSegmentId });
            builder.ToTable(t=>t.HasCheckConstraint("CHK_TripExecution_Departure_Arrival",
            @"""Arrival"" >= ""Departure"""));
        }
    }
}
