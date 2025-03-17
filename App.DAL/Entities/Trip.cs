using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Entities
{
    // определяет конкретный выезд по маршруту
    public class Trip
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public Route? Route { get; set; }
        public int RouteScheduleId { get; set; }
        public RouteSchedule? RouteSchedule { get; set; }
        public int BusId { get; set; }
        public Bus? Bus { get; set; }
        public int DriverId { get; set; }
        public Driver? Driver { get; set; }
        public DateTimeOffset Departure {  get; set; }
        public string? FactualSeatingPlan { get; set; } // денормализация
    }

    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.Property(p => p.FactualSeatingPlan)
                .IsRequired();
        }
    }
}
