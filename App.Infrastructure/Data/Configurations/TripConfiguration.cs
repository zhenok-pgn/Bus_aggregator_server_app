using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.Property(p => p.FactualSeatingPlan)
                .IsRequired();
        }
    }
}
