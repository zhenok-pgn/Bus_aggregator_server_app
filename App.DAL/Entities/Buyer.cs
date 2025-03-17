namespace App.DAL.Entities
{
    public class Buyer
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
