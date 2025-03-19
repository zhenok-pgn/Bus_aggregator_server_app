using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();
        }
    }
}
