using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class LocalityConfiguration : IEntityTypeConfiguration<Locality>
    {
        public void Configure(EntityTypeBuilder<Locality> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();
        }
    }
}
