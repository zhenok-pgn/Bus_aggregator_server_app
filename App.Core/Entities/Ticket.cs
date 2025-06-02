namespace App.Core.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public Passenger? Passenger { get; set; }
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
