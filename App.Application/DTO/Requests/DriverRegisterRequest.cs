namespace App.Application.DTO.Requests
{
    public class DriverRegisterRequest : LoginRequest
    {
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public required string Patronymic { get; set; }
        public required string LicenseNumber { get; set; }
        public required string EmployeeNumber { get; set; }
        public required string DayOfBirth { get; set; }
    }
}
