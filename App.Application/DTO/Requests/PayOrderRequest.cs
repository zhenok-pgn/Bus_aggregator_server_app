namespace App.Application.DTO.Requests
{
    public class PayOrderRequest
    {
        public required OrderNumber OrderNumber { get; set; }
        public required List<PassengerDTO> Passengers { get; set; }
    }
}
