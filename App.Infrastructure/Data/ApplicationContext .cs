using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using App.Core.Entities;
using App.Infrastructure.Data.Configurations;

namespace App.Infrastructure.Data
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Carrier> Carriers { get; set; } = null!;
        public DbSet<Bus> Buses { get; set; } = null!;
        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<Locality> Localities { get; set; } = null!;
        public DbSet<RouteSegmentPrice> Prices { get; set; } = null!;
        public DbSet<Route> Routes { get; set; } = null!;
        public DbSet<RouteStop> RouteStops { get; set; } = null!;
        public DbSet<RouteSchedule> RouteSchedules { get; set; } = null!;
        public DbSet<BusStop> BusStops { get; set; } = null!;
        public DbSet<Tariff> Tariffs { get; set; } = null!;
        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<Passenger> Passengers { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Host=localhost;Port=5432;Database=postgres;Username=zhenya;Password=";  
                optionsBuilder.UseNpgsql(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        }

        private readonly string connectionString;

        public ApplicationDBContext(string connectionString = "Host=localhost;Port=5432;Database=postgres;Username=zhenya;Password=")    //"server=localhost;user=root;password=root;database=ef") -- for mysql
        {
            this.connectionString = connectionString;
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));

            optionsBuilder
                //.UseMySql(connectionString, serverVersion)
                .UseNpgsql(connectionString)
                // The following three options help with debugging, but should                                                    
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RouteConfiguration());
            modelBuilder.ApplyConfiguration(new TripConfiguration());
            modelBuilder.ApplyConfiguration(new BusStopConfiguration());
            modelBuilder.ApplyConfiguration(new LocalityConfiguration());
            modelBuilder.ApplyConfiguration(new TariffConfiguration());
            modelBuilder.ApplyConfiguration(new RouteScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new BusConfiguration());
            modelBuilder.ApplyConfiguration(new DriverConfiguration());
            modelBuilder.ApplyConfiguration(new CarrierConfiguration());
            modelBuilder.ApplyConfiguration(new PassengerConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}
