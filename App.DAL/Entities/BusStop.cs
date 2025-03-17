using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Entities
{
    /// <summary>
    /// Остановка или станция, находится в населенном пункте (Locality)
    /// </summary>
    public class BusStop
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public StationType Type { get; set; }
        public string? Address { get; set; }
        public int LocalityId { get; set; }
        public Locality? Locality { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }

    public class StationConfiguration : IEntityTypeConfiguration<BusStop>
    {
        public void Configure(EntityTypeBuilder<BusStop> builder)
        {
            builder.Property(p => p.Address)
                .IsRequired();
        }
    }

    public enum StationType
    {
        Station,
        Terminal,
        Stop,
        Other
    }
}
