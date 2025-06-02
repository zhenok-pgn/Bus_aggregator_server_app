using App.Core.Entities;

namespace App.Application.DTO
{
    public class BusStopDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required LocalityDTO Locality { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}