using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Entities
{
    public class Passenger
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public bool IsBan { get; set; }
    }

    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.Property(p => p.FirstName)
                .IsRequired();
            builder.Property(p => p.LastName)
                .IsRequired();
            builder.Property(p => p.Phone)
                .IsRequired();
            builder.Property(p => p.Password)
                .IsRequired();
        }
    }
}
