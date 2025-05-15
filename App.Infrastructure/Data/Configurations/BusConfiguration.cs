using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class BusConfiguration : IEntityTypeConfiguration<Bus>
    {
        public void Configure(EntityTypeBuilder<Bus> builder)
        {
            builder.HasAlternateKey(b => new { b.Vin });
            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Bus_Vin_Length", @"length(""Vin"") = 17"));
        }
    }
}
