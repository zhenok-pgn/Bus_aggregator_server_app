using App.Core.Entities;

namespace App.Application.DTO
{
    public class BookingDTO
    {
        public required string Id { get; set; }
        public required string SeatNumber { get; set; }
        public required string BookingStatus { get; set; }
        public TicketDTO? Ticket { get; set; }
    }
}
