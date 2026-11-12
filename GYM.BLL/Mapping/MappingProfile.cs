using AutoMapper;
using GYM.BLL.ViewModels;
using GYM.DAL.Data.Models;

namespace GYM.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ---------- Members ----------
            CreateMap<Member, MemberView>()
                .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToString("dd MMM yyyy")))
                .ForMember(d => d.Address, o => o.MapFrom(s => FormatAddress(s.Address)));

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Photo, o => o.Ignore())
                .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender!.Value))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth!.Value))
                .ForMember(d => d.Address, o => o.MapFrom(s => new Address
                {
                    BuildingNomber = s.BuildingNumber,
                    City = s.City,
                    Street = s.Street
                }))
                .ForMember(d => d.HealthRecord, o => o.MapFrom(s => s.HealthRecordViewModel))
                .ForMember(d => d.memberships, o => o.Ignore())
                .ForMember(d => d.Bookings, o => o.Ignore())
                .ForMember(d => d.CreateAt, o => o.Ignore())
                .ForMember(d => d.UpdateAt, o => o.Ignore());

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNomber))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
                .ForMember(d => d.PhotoFile, o => o.Ignore());

            CreateMap<HealthRecordViewModel, HealthRecord>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.MemberId, o => o.Ignore())
                .ForMember(d => d.Member, o => o.Ignore())
                .ForMember(d => d.CreateAt, o => o.Ignore())
                .ForMember(d => d.UpdateAt, o => o.Ignore());

            CreateMap<HealthRecord, HealthRecordViewModel>();

            // ---------- Trainers ----------
            CreateMap<Trainer, TrainerView>()
                .ForMember(d => d.Specialties, o => o.MapFrom(s => s.Specialtiy.ToString()))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToString("dd MMM yyyy")))
                .ForMember(d => d.Address, o => o.MapFrom(s => FormatAddress(s.Address)));

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Photo, o => o.Ignore())
                .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender!.Value))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth!.Value))
                .ForMember(d => d.Specialtiy, o => o.MapFrom(s => s.Specialties!.Value))
                .ForMember(d => d.Address, o => o.MapFrom(s => new Address
                {
                    BuildingNomber = s.BuildingNumber,
                    City = s.City,
                    Street = s.Street
                }))
                .ForMember(d => d.Sessions, o => o.Ignore())
                .ForMember(d => d.CreateAt, o => o.Ignore())
                .ForMember(d => d.UpdateAt, o => o.Ignore());

            CreateMap<Trainer, EditTrainerViewModel>()
                .ForMember(d => d.Specialties, o => o.MapFrom(s => s.Specialtiy))
                .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNomber))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
                .ForMember(d => d.PhotoFile, o => o.Ignore());

            // ---------- Plans ----------
            CreateMap<Plan, PlanView>();

            // ---------- Memberships ----------
            CreateMap<Membership, MemberShipViewModel>()
                .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member.Name))
                .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.CreateAt));

            // ---------- Sessions ----------
            CreateMap<Session, SessionViewModel>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.CategoryName))
                .ForMember(d => d.TrainerName, o => o.MapFrom(s => s.Trainer.Name))
                .ForMember(d => d.AvailableSlots, o => o.MapFrom(s => s.Capacity - (s.bookings == null ? 0 : s.bookings.Count)))
                .ForMember(d => d.Status, o => o.MapFrom(s => GetStatus(s.StartDate, s.EndDate)))
                .ForMember(d => d.Duration, o => o.MapFrom(s => FormatDuration(s.EndDate - s.StartDate)))
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

            // ---------- Bookings ----------
            CreateMap<Booking, MemberForSessionViewModel>()
                .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member.Name))
                .ForMember(d => d.BookingDate, o => o.MapFrom(s => s.BookingDate.ToString("dd MMM yyyy, hh:mm tt")));
        }

        public static string GetStatus(DateTime start, DateTime end)
        {
            var now = DateTime.Now;
            if (now < start) return "Upcoming";
            if (now <= end) return "Ongoing";
            return "Completed";
        }

        private static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalMinutes <= 0) return "0m";
            var hours = (int)duration.TotalHours;
            return hours > 0
                ? (duration.Minutes > 0 ? $"{hours}h {duration.Minutes}m" : $"{hours}h")
                : $"{duration.Minutes}m";
        }

        private static string FormatAddress(Address? address)
            => address is null ? string.Empty : $"{address.BuildingNomber} {address.Street}, {address.City}";
    }
}
