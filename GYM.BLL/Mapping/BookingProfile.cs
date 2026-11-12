using AutoMapper;
using GYM.BLL.ViewModels.Bookings;
using GYM.DAL.Data.Models;

namespace GYM.BLL.Mapping
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, MemberForSessionViewModel>()
                .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member.Name))
                .ForMember(d => d.BookingDate, o => o.MapFrom(s => s.BookingDate.ToString("dd MMM yyyy, hh:mm tt")));
        }
    }
}
