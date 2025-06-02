using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class TripMappingProfile : Profile
    {
        public TripMappingProfile()
        {
            CreateMap<TripPlanRequest, Trip>();

            CreateMap<Trip, TripDTO>()
            .ForMember(dest => dest.Route, opt => opt.MapFrom(src => new RouteSummaryDTO
            {
                Id = src.RouteSchedule.Id.ToString(),
                Name = src.RouteSchedule.Route.Name,
                RegistrationNumber = src.RouteSchedule.Route.RegistrationNumber,
                Number = src.RouteSchedule.Route.Number,
                CarrierId = src.RouteSchedule.Route.CarrierId
            }))
            .ForMember(dest => dest.TripStatus, opt => opt.MapFrom(src => src.TripStatus.ToString()));
            
            CreateMap<TripDTO, Trip>();
        }
    }
}
