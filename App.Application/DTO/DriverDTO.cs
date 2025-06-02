namespace App.Application.DTO
{
    public class DriverDTO
    {
        public required string Id { get; set; }
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public required string Patronymic { get; set; }
        public required string LicenseNumber { get; set; }
        public required string EmployeeNumber { get; set; }
        public DateOnly DayOfBirth { get; set; }
        public int CarrierId { get; set; }
        public required string UserName { get; set; }
    }
}
