using App.Application.DTO;
using App.Application.DTO.Requests;

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
            await _authService.RegisterCarrierAsync(new CarrierRegisterRequest()
            {
                Address = "г. Москва, ул. Примерная, д. 1, кв. 1",
                Email = "example@mail.com",
                Inn = "1234567890",
                Name = "ИП Минин",
                Ogrn = "1027700000000",
                Phone = "+7 (495) 123-45-67",
                Password = request.Password,
                Username = request.Username
            });

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