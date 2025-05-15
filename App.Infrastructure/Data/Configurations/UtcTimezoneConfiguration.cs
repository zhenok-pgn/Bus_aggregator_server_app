using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class UtcTimezoneConfiguration : IEntityTypeConfiguration<UtcTimezone>
    {
        public void Configure(EntityTypeBuilder<UtcTimezone> builder)
        {
            builder.HasIndex(x => x.Name)
            .IsUnique();
            builder.ToTable(t => {
                t.HasCheckConstraint("CK_UtcTimezone_Offset", @"""OffsetMinutes"" BETWEEN -840 AND 840");
            });
        }
    }
}
