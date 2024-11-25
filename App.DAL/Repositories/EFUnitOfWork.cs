using App.DAL.EF;
using App.DAL.Entities;
using App.DAL.Interfaces;

namespace App.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationMysqlContext db;
        private CarrierRepository? carrierRepository;

        public EFUnitOfWork(string connectionString = "server=localhost;user=root;password=root;database=ef")
        {
            db = new ApplicationMysqlContext(connectionString);
        }
        public IRepository<Carrier> Carriers
        {
            get
            {
                if (carrierRepository == null)
                    carrierRepository = new CarrierRepository(db);
                return carrierRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
