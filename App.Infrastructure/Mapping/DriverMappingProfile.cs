using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Core.Entities;
using App.Core.Enums;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class DriverMappingProfile : Profile
    {
        public DriverMappingProfile()
        {
            CreateMap<Driver, DriverDTO>();
            CreateMap<DriverRegisterRequest, Driver>();
        }
    }
}
