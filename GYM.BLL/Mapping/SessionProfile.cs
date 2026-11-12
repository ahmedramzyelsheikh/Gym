using AutoMapper;
using GYM.BLL.ViewModels;
using GYM.DAL.Data.Models;

namespace GYM.BLL.Mapping
{
    public class SessionProfile : Profile
    {
        public SessionProfile()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.CategoryName))
                .ForMember(d => d.TrainerName, o => o.MapFrom(s => s.Trainer.Name))
                .ForMember(d => d.AvailableSlots, o => o.MapFrom(s => s.Capacity - (s.bookings == null ? 0 : s.bookings.Count)))
                .ForMember(d => d.Status, o => o.MapFrom(s => MappingHelpers.GetStatus(s.StartDate, s.EndDate)))
                .ForMember(d => d.Duration, o => o.MapFrom(s => MappingHelpers.FormatDuration(s.EndDate - s.StartDate)))
                .ForMember(d => d.DateDisplay, o => o.MapFrom(s => s.StartDate.ToString("ddd, MMM dd yyyy")))
                .ForMember(d => d.TimeRangeDisplay, o => o.MapFrom(s => $"{s.StartDate:hh:mm tt} - {s.EndDate:hh:mm tt}"));

            CreateMap<Session, UpdateSessionViewModel>();

            CreateMap<CreateSessionViewModel, Session>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.bookings, o => o.Ignore())
                .ForMember(d => d.Trainer, o => o.Ignore())
                .ForMember(d => d.Category, o => o.Ignore())
                .ForMember(d => d.CreateAt, o => o.Ignore())
                .ForMember(d => d.UpdateAt, o => o.Ignore());

            CreateMap<Category, CategoryViewModel>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.CategoryName));
        }
    }
}
