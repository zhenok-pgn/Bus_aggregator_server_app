using App.DAL.Entities;

namespace App.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Carrier> Carriers { get; }
        void Save();
    }
}
