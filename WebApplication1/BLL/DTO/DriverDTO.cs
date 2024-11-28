using App.BLL.Infrastructure;

namespace App.BLL.DTO
{
    public class DriverDTO
    {
        public int Id { get; set; }
        public required string LicenseId { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required Role Role { get; set; }
    }
}
