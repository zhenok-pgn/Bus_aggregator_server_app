using App.Application.DTO;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class PassengerMappingProfile : Profile
    {
        public PassengerMappingProfile()
        {
            CreateMap<Passenger, PassengerDTO>();
            CreateMap<PassengerDTO, Passenger>();
        }
    }
}
