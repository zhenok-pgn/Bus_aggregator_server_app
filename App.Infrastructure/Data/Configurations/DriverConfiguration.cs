using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            //builder.HasKey(e => e.Id).HasName("pk_driver");
            builder.HasIndex(d => d.LicenseNumber).IsUnique();
            builder.HasIndex(d => new { d.EmployeeNumber, d.CarrierId }).IsUnique();
        }
    }
}
