namespace App.Application.DTO
{
    public class TripDTO
    {
        public int Id { get; set; }
        public required CarrierDTO Carrier { get; set; }
    }
}
