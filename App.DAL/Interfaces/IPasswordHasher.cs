namespace App.DAL.Interfaces
{
    public interface IPasswordHasher<T>
    {
        static abstract T HashPassword(string password);
        static abstract bool VerifyPassword(string password, T hash);
    }
}
