namespace App.Application.DTO
{
    public class BusDTO
    {
        public required string Id { get; set; }
        public required string Model { get; set; }
        public required string StateNumber { get; set; }
        public required string Vin { get; set; }
        public int CarrierId { get; set; }
        public required List<string> Seats { get; set; }
    }
}
