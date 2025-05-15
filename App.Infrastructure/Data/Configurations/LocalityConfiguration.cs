using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class LocalityConfiguration : IEntityTypeConfiguration<Locality>
    {
        public void Configure(EntityTypeBuilder<Locality> builder)
        {
            builder.HasAlternateKey(l => l.Okato);
            builder.Property(l => l.Okato).HasMaxLength(11);
            builder.Property(l => l.Name).HasMaxLength(100);
        }
    }
}
