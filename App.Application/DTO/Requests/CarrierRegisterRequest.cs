namespace App.Application.DTO.Requests
{
    public class CarrierRegisterRequest : LoginRequest
    {
        public required string Name { get; set; }
        public required string Inn { get; set; }
        public required string Ogrn { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
    }
}
