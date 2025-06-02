using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            //builder.HasAlternateKey(t => new { t.Series, t.Number });
            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Ticket_Price_Positive", @"""Price"" >= 0"));
        }
    }
}
