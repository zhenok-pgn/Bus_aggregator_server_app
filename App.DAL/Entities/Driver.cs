using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Entities
{
    public class Driver
    {
        public int Id { get; set; }
        public required string LicenseId { get; set; }
        public required string Name { get; set; }
        public bool IsBan { get; set; }
        public required string HashedPassword { get; set; }
    }

    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();
        }
    }
}
