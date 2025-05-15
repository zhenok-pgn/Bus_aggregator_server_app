namespace App.Application.DTO.Requests
{
    public class CreateOrderRequest
    {
        public int TripId {  get; set; }

        public int SegmentId { get; set; }

        public required List<int> SeatIds { get; set; }
    }
}
