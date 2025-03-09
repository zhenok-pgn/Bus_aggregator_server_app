using App.DAL.Entities;
using App.DAL.Interfaces;
using App.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repositories
{
    internal class CarrierRepository : IRepository<Carrier>
    {
        private ApplicationDBContext db;

        public CarrierRepository(ApplicationDBContext context)
        {
            db = context;
        }

        public void Create(Carrier item)
        {
            db.Carriers.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Carriers.Find(id);
            if (item != null)
                db.Carriers.Remove(item); ;
        }

        public IEnumerable<Carrier> Find(Func<Carrier, bool> predicate)
        {
            return db.Carriers.Where(predicate).ToList();
        }

        public Carrier Get(int id)
        {
            return db.Carriers.Find(id);
        }

        public IEnumerable<Carrier> GetAll()
        {
            return db.Carriers;
        }

        public void Update(Carrier item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
