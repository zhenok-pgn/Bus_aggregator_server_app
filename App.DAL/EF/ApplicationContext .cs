using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using App.DAL.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.DAL.EF
{
    public class ApplicationMysqlContext : DbContext
    {
        public DbSet<Carrier> Carriers { get; set; } = null!;
        public DbSet<Bus> Buses { get; set; } = null!;
        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<Locality> Localities { get; set; } = null!;
        public DbSet<Price> Prices { get; set; } = null!;
        public DbSet<RoutePoint> RoutePoints { get; set; } = null!;
        public DbSet<RouteSchedule> RouteSchedules { get; set; } = null!;
        public DbSet<Station> Stations { get; set; } = null!;
        public DbSet<Tariff> Tariffs { get; set; } = null!;
        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<Passenger> Passengers { get; set; } = null!;

        private readonly string connectionString;

        public ApplicationMysqlContext(string connectionString = "server=localhost;user=root;password=root;database=ef")
        {
            Database.EnsureCreated();
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));

            optionsBuilder
                .UseMySql(connectionString, serverVersion)
                // The following three options help with debugging, but should                                                    
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RouteConfiguration());
            modelBuilder.ApplyConfiguration(new TripConfiguration());
            modelBuilder.ApplyConfiguration(new StationConfiguration());
            modelBuilder.ApplyConfiguration(new LocalityConfiguration());
            modelBuilder.ApplyConfiguration(new TariffConfiguration());
            modelBuilder.ApplyConfiguration(new RouteScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new BusConfiguration());
            modelBuilder.ApplyConfiguration(new DriverConfiguration());
            modelBuilder.ApplyConfiguration(new CarrierConfiguration());
            modelBuilder.ApplyConfiguration(new PassengerConfiguration());
        }
    }
}
