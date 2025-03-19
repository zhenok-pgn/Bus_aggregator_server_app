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
            builder.Property(p => p.Name)
                .IsRequired();

            builder.HasMany(r => r.RouteStops)
                .WithOne(rs => rs.Route)
                .HasForeignKey(rs => rs.RouteId);

            builder.HasMany(r => r.RouteSchedules)
                .WithOne(rs => rs.Route)
                .HasForeignKey(rs => rs.RouteId);

            builder.HasMany(r => r.Tariffs)
                .WithOne(rs => rs.Route)
                .HasForeignKey(rs => rs.RouteId);
        }
    }
}
