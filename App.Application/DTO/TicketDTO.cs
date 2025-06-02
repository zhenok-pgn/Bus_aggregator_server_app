using App.Core.Entities;

namespace App.Application.DTO
{
    public class TicketDTO
    {
        public required string Id { get; set; }
        public required PassengerDTO Passenger { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}