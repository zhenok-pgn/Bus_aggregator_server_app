using App.Application.DTO;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class BusMappingProfile : Profile
    {
        public BusMappingProfile()
        {
            CreateMap<Bus, BusDTO>().ForMember(b=>b.Seats, opt=>opt.MapFrom(src =>
                src.Seats.Select(s => s.SeatNumber).ToList()))
                .ForMember(dest => dest.Seats, opt => opt.MapFrom(src =>
                src.Seats.Select(s => s.SeatNumber).ToList()));
            CreateMap<BusDTO, Bus>().ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                IdParser.SafeParseId(src.Id)))
                .ForMember(dest => dest.Seats, opt => opt.MapFrom(src =>
        src.Seats.Select(seatNumber => new Seat { SeatNumber = seatNumber }).ToList()));
        }
    }
}
