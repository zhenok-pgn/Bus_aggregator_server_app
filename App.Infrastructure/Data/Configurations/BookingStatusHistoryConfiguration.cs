using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Data.Configurations
{
    class BookingStatusHistoryConfiguration : IEntityTypeConfiguration<BookingStatusHistory>
    {
        public void Configure(EntityTypeBuilder<BookingStatusHistory> builder)
        {
            builder.HasIndex(b => new { b.BookingId, b.Status })
                .IsUnique();

        }
    }
}
