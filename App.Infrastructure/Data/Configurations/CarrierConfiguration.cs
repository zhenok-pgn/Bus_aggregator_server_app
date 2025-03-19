using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class CarrierConfiguration : IEntityTypeConfiguration<Carrier>
    {
        public void Configure(EntityTypeBuilder<Carrier> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();
            builder.Property(p => p.Inn)
                .IsRequired();
            builder.Property(p => p.Ogrn)
                .IsRequired();
            builder.Property(p => p.Ogrnip)
                .IsRequired();
            builder.Property(p => p.Address)
                .IsRequired();
        }
    }
}
