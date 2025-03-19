using App.Application.DTO;
using App.Core.Entities;

namespace App.Application.Mapping
{

    public static class MappingExtensions
    {
        /*public static List<toT> MapToDto<fromT, toT>(this List<fromT>? mappingObjects) where fromT : class
        {
            ArgumentNullException.ThrowIfNull(mappingObjects);

            return mappingObjects.Select(obj => obj.MapToDto<toT>()).ToList();
        }

        public static T1 MapToDto<T1>(this object? mappingObject)
        {
            ArgumentNullException.ThrowIfNull(mappingObject);

            return mappingObject switch
            {
                Passenger passenger => (T1)(object)new PassengerDTO(
                    passenger.Id,
                    passenger.FirstName,
                    passenger.LastName,
                    // TODO: тут заглушка если что
                    false,
                    "passenger.HashedPassword",
                    new Role("passenger")
                ),
                Carrier carrier => (T1)(object)new CarrierDTO(
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
                    ),
                Driver driver => (T1)(object)new DriverDTO()
                {
                    Id = driver.Id,
                    LicenseId = driver.LicenseId,
                    Name = driver.Name,
                    Password = driver.HashedPassword,
                    Role = new Role("driver")
                },
                Route route => (T1)(object)new RouteDTO()
                {
                    Id = route.Id,
                },
                Trip trip => (T1)(object)new TripDTO()
                {
                    Id = trip.Id,
                },
                _ => throw new ArgumentException("Unsupported mapping type", nameof(mappingObject))
            };
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
        }*/
    }
}
