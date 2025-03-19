using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.Property(p => p.FirstName)
                .IsRequired();
            builder.Property(p => p.LastName)
                .IsRequired();
        }
    }
}
