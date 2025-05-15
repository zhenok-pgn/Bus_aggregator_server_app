namespace App.Application.DTO.Requests
{
    public class PassengerRegisterRequest : LoginRequest
    {
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
    }
}
