namespace App.DAL.Interfaces
{
    public interface IPasswordHasher
    {
        static abstract string HashPassword(string password);
        static abstract bool VerifyPassword(string password, string hash);
    }
}
