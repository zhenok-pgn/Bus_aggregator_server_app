namespace App.Core.Entities
{
    public class Bus
    {
        public int Id { get; set; }
        public required string Model { get; set; }
        public required string StateNumber { get; set; }
        public required string Vin { get; set; }
        public int CarrierId { get; set; }
        public Carrier? Carrier { get; set; }
        public List<Seat> Seats { get; set; } = new();
    }
}
