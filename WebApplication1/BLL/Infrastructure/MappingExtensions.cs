using App.BLL.DTO;
using App.DAL.Entities;
using App.BLL.Infrastructure;
using System.ComponentModel;

namespace App.WEB.BLL.Infrastructure
{

    public static class MappingExtensions
    {
        public static List<T1> MapToDto<T1, T2>(this List<T2>? mappingObjects) where T2 : class
        {
            if (mappingObjects == null)
                throw new ArgumentNullException(nameof(mappingObjects));

            // Используем LINQ для преобразования каждого элемента списка
            return mappingObjects.Select(obj => MapToDto<T1>(obj)).ToList();
        }

        private static T1 MapToDto<T1>(this object mappingObject)
        {
            // Здесь вы можете использовать рефлексию или другие методы для определения
            // соответствующего метода преобразования на основе типа T2.
            // Для простоты примера, я предполагаю, что у вас есть методы преобразования
            // для каждого типа.

            if (mappingObject is Passenger passenger)
            {
                return (T1)(object)new PassengerDTO(
                    passenger.Id,
                    passenger.FirstName,
                    passenger.LastName,
                    passenger.Phone,
                    passenger.IsBan,
                    passenger.HashedPassword,
                    new Role("passenger")
                );
            }
            else if (mappingObject is Carrier carrier)
            {
                return (T1)(object)new CarrierDTO(
                    carrier.Id,
                    carrier.Name,
                    carrier.Inn,
                    carrier.Ogrn,
                    carrier.Ogrnip,
                    carrier.Address,
                    carrier.OfficeHours,
                    carrier.Phones,
                    carrier.HashedPassword,
                    new Role("carrier")
                );
            }
            else if (mappingObject is Driver driver)
            {
                return (T1)(object)new DriverDTO()
                {
                    Id = driver.Id,
                    LicenseId = driver.LicenseId,
                    Name = driver.Name,
                    Password = driver.HashedPassword,
                    Role = new Role("driver")
                };
            }

            throw new InvalidOperationException($"No mapping defined for type {mappingObject.GetType().Name}");
        }
        public static PassengerDTO MapToDto(this Passenger? mappingObject)
        {
            if (mappingObject == null)
                throw new ArgumentNullException(nameof(mappingObject));

            var dest = new PassengerDTO(
                mappingObject.Id,
                mappingObject.FirstName,
                mappingObject.LastName,
                mappingObject.Phone,
                mappingObject.IsBan,
                mappingObject.HashedPassword,
                new Role("passenger")
                );

            return dest;
        }

        public static CarrierDTO MapToDto(this Carrier? mappingObject)
        {
            if (mappingObject == null)
                throw new ArgumentNullException(nameof(mappingObject));

            var dest = new CarrierDTO(
                mappingObject.Id,
                mappingObject.Name,
                mappingObject.Inn,
                mappingObject.Ogrn,
                mappingObject.Ogrnip,
                mappingObject.Address,
                mappingObject.OfficeHours,
                mappingObject.Phones,
                mappingObject.HashedPassword,
                new Role("carrier")
                );

            return dest;
        }

        public static DriverDTO MapToDto(this Driver? mappingObject)
        {
            if (mappingObject == null)
                throw new ArgumentNullException(nameof(mappingObject));

            var dest = new DriverDTO() { 
                Id = mappingObject.Id,
                LicenseId = mappingObject.LicenseId,
                Name = mappingObject.Name,
                Password = mappingObject.HashedPassword,
                Role = new Role("driver")
                };

            return dest;
        }
    }
}
