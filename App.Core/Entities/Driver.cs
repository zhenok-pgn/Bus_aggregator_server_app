namespace App.Core.Entities
{
    public class Driver : User
    {
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public required string Patronymic { get; set; }
        public required string LicenseNumber { get; set; }
        public required string EmployeeNumber { get; set; }
        public DateOnly DayOfBirth { get; set; }
        public int CarrierId { get; set; }
        public Carrier? Carrier { get; set; }
    }
}
