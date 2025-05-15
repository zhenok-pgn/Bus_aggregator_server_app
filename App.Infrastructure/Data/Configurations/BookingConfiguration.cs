using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasIndex(b => new { b.TripId, b.SeatId, b.RouteSegmentScheduleId, b.OrderCreated })
                .IsUnique();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Booking_Expires", @"""Expires"" IS NULL OR ""Expires"" >= ""OrderCreated""");
            });
        }
    }
}
