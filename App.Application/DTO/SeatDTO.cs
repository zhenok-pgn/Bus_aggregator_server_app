namespace App.Application.DTO
{
    public class SeatDTO
    {
        public required int Id { get; set; }
        public required string SeatNumber { get; set; }
        public bool IsAvailable { get; set; }
    }
}
