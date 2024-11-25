using App.BLL.DTO;
using App.DAL.Entities;
using App.BLL.Infrastructure;

namespace App.WEB.BLL.Infrastructure
{
    public static class MappingExtensions
    {
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
                new Role("passenger")
                );

            return dest;

        }
    }
}
