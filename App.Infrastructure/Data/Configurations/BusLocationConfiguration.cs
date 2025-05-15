using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class BusLocationConfiguration : IEntityTypeConfiguration<BusLocation>
    {
        public void Configure(EntityTypeBuilder<BusLocation> builder)
        {
            builder.HasKey(bl => new { bl.Latitude, bl.Longitude, bl.Timestamp });
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Bus_Location_Latitude", @"""Latitude"" BETWEEN -90 AND 90");
                t.HasCheckConstraint("CK_Bus_Location_Longitude", @"""Longitude"" BETWEEN -180 AND 180");
            });
        }
    }
}
