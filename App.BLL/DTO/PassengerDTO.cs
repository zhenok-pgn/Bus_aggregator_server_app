using App.BLL.Infrastructure;

namespace App.BLL.DTO
{
    public class PassengerDTO(int id, string firstName, string lastName, string phone, bool isBan, Role role)
    {
        public int Id { get; set; } = id;
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string Phone { get; set; } = phone;
        public bool IsBan { get; set; } = isBan;
        public Role Role { get; set; } = role;
    }
}
