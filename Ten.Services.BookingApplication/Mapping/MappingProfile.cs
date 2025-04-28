using AutoMapper;
using Ten.Services.BookingApplication.Dtos;
using Ten.Services.BookingDomain.Entities;

namespace Ten.Services.BookingApplication.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Booking, BookingDto>().ReverseMap();
            CreateMap<Member, BookingDto>().ReverseMap();
        }
    }
}
