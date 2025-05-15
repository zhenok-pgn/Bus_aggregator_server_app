using App.Application.DTO;

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
            using (ApplicationDBContext db = new ApplicationDBContext())
            {
                Locality user1 = new Locality { Name = "Tom"};
                Locality user2 = new Locality { Name = "Alice" };

                db.Localities.AddRange(user1, user2);
                db.SaveChanges();
            }
            Assert.Pass();
        }
    }
}