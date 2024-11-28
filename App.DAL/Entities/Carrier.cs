using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Entities
{
    public class Carrier
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Inn { get; set; }
        public required string Ogrn { get; set; }
        public required string Ogrnip { get; set; }
        public required string Address { get; set; }
        public required string OfficeHours { get; set; }
        public required string Phones { get; set; }
        public bool IsBan { get; set; }
        public required string HashedPassword {  get; set; } 
    }

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
