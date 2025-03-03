using App.BLL.DTO;
using App.DAL.EF;
using App.DAL.Entities;
using App.WEB.BLL.Infrastructure;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            using (ApplicationMysqlContext db = new ApplicationMysqlContext())
            {
                Locality user1 = new Locality { Name = "Tom"};
                Locality user2 = new Locality { Name = "Alice" };

                db.Localities.AddRange(user1, user2);
                db.SaveChanges();
            }
            Assert.Pass();
        }

        [Test]
        public void TestMapping()
        {
            Driver driver1 = new Driver() { LicenseId = "123", Name = "Name1", HashedPassword = "123456789" };
            Driver driver2 = new Driver() { LicenseId = "132", Name = "Name2", HashedPassword = "1234567892424" };
            var list = new List<Driver> { driver1, driver2 };

            var a = driver1.MapToDto<DriverDTO>();
            var b = list.MapToDto<Driver, DriverDTO>();
            Assert.Pass();
        }
    }
}