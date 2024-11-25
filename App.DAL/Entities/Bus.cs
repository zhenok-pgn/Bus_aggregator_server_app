using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Entities
{
    public class Bus
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? StateNumber { get; set; }
        public string? Vin {  get; set; }
        public string? Color { get; set; }
        public string? SeatingPlan { get; set; }
        public Comforts? Comforts { get; set; }
    }

    [Owned]
    public class Comforts
    {
        public bool? HasToilet { get; set; }
        public bool? HasClimateControl { get; set; }
        public bool? HasWiFi { get; set; }
        public bool? HasRosette { get; set; }
        public bool? HasFoldingSeats { get; set; }
    }

    public class BusConfiguration : IEntityTypeConfiguration<Bus>
    {
        public void Configure(EntityTypeBuilder<Bus> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();
            builder.Property(p => p.StateNumber)
                .IsRequired();
            builder.Property(p => p.Vin)
                .IsRequired();
        }
    }
}
