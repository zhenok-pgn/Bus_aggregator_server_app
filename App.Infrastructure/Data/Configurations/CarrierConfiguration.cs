using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class CarrierConfiguration : IEntityTypeConfiguration<Carrier>
    {
        public void Configure(EntityTypeBuilder<Carrier> builder)
        {
            //builder.HasKey(e => e.Id).HasName("pk_carrier");
            builder.HasIndex(c => c.Inn).IsUnique();
            builder.HasIndex(c => c.Ogrn).IsUnique();
            builder.HasIndex(c => c.Phone).IsUnique();
            builder.HasIndex(c => c.Email).IsUnique();
            builder.ToTable(t => {
                t.HasCheckConstraint("CK_Carrier_Inn_Format", @"""Inn"" ~ '^[0-9]{10}$'");
                t.HasCheckConstraint("CK_Carrier_Ogrn_Format", @"""Ogrn"" ~ '^[0-9]{13}$'");
                t.HasCheckConstraint("CK_Carrier_Email_Format", @"""Email"" ~* '^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,}$'");
            });
        }
    }
}
