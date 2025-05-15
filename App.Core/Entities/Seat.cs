namespace App.Core.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int BusId { get; set; }
        public Bus? Bus { get; set; }
        public required string SeatNumber { get; set; }
        public bool RemovedFromSeatingPlan { get; set; }
    }
}
