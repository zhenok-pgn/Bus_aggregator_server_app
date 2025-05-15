using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            //builder.Property(p => p.Id)
            //    .HasColumnType("bigint"); // сделать bigint
            builder.HasIndex(r => r.RegistrationNumber)
            .IsUnique();
            builder.HasIndex(r => new { r.Number, r.CarrierId })
            .IsUnique();

        }
    }
}
