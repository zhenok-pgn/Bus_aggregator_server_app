using App.DAL.EF;
using App.DAL.Entities;

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
                Carrier user1 = new Carrier { Name = "Tom"};
                Carrier user2 = new Carrier { Name = "Alice" };

                db.Carriers.AddRange(user1, user2);
                db.SaveChanges();
            }
            Assert.Pass();
        }
    }
}