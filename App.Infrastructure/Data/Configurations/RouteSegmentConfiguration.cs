using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class RouteSegmentConfiguration : IEntityTypeConfiguration<RouteSegment>
    {
        public void Configure(EntityTypeBuilder<RouteSegment> builder)
        {
            builder.HasAlternateKey(rs => new { rs.FromStopId, rs.ToStopId });
            builder
                .HasOne(rs => rs.FromStop)
                .WithMany()
                .HasForeignKey(rs => rs.FromStopId)
                .OnDelete(DeleteBehavior.Restrict); // устранение циклического каскадного удаления

            builder
                .HasOne(rs => rs.ToStop)
                .WithMany()
                .HasForeignKey(rs => rs.ToStopId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
