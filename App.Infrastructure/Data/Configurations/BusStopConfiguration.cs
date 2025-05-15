using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class BusStopConfiguration : IEntityTypeConfiguration<BusStop>
    {
        public void Configure(EntityTypeBuilder<BusStop> builder)
        {
            builder.HasAlternateKey(b => new {b.Latitude, b.Longitude});
            builder.ToTable(t => 
            { 
                t.HasCheckConstraint("CK_BusStop_Latitude", @"""Latitude"" BETWEEN -90 AND 90");
                t.HasCheckConstraint("CK_BusStop_Longitude", @"""Longitude"" BETWEEN -180 AND 180");
            });
        }
    }
}
