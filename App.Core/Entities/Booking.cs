using App.Core.Enums;

namespace App.Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public Buyer? Buyer { get; set; }
        public int RouteSegmentScheduleId { get; set; }
        public RouteSegmentSchedule? RouteSegmentSchedule { get; set; }
        public int SeatId { get; set; }
        public Seat? Seat { get; set; }
        public int TripId { get; set; }
        public Trip? Trip { get; set; }
        public DateTimeOffset OrderCreated { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public List<BookingStatusHistory>? BookingStatusHistories { get; set; }
    }
}
