using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class BusConfiguration : IEntityTypeConfiguration<Bus>
    {
        public void Configure(EntityTypeBuilder<Bus> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();
            builder.Property(p => p.StateNumber)
                .IsRequired();
            builder.Property(p => p.Vin)
                .IsRequired();
        }
    }
}
