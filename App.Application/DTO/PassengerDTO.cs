using App.Core.Entities;

namespace App.Application.DTO
{
    public class PassengerDTO(int id, string firstName, string lastName, string phone, bool isBan, string password, Role role)
    {
        public int Id { get; set; } = id;
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string Phone { get; set; } = phone;
        public bool IsBan { get; set; } = isBan;
        public string Password { get; set; } = password;
        public Role Role { get; set; } = role;
    }
}
