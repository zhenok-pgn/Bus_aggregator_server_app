using App.Core.Entities;

namespace App.Application.DTO
{
    public class CarrierDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Inn { get; set; } = string.Empty;
        public string Ogrn { get; set; } = string.Empty;
        public string Ogrnip { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string OfficeHours { get; set; } = string.Empty;
        public string Phones { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; } = new("carrier");
    }
}
