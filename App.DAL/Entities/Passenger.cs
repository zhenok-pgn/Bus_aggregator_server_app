using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Entities
{
    public class Passenger
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PassportNumber { get; set; }
        public required string Sex { get; set; }
    }

    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.Property(p => p.FirstName)
                .IsRequired();
            builder.Property(p => p.LastName)
                .IsRequired();
        }
    }
}
