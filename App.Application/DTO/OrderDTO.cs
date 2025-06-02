namespace App.Application.DTO
{
    public class OrderDTO
    {
        public required OrderNumber OrderNumber { get; set; }
        public required TripDTO Trip { get; set; }
        public required RouteSegmentScheduleDTO RouteSegmentSchedule { get; set; }
        public required List<BookingDTO> Bookings { get; set; }
    }
}