using App.Core.Enums;

namespace App.Core.Entities
{
    public class BookingStatusHistory
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }
        public BookingStatus Status { get; set; }
        public DateTimeOffset StatusChangedAt { get; set; }
    }
}
