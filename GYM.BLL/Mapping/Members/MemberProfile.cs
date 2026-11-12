using AutoMapper;
using GYM.BLL.ViewModels.Members;
using GYM.DAL.Data.Models;
using GYM.BLL.Mapping.Common;

namespace GYM.BLL.Mapping.Members
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberView>()
                .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToString("dd MMM yyyy")))
                .ForMember(d => d.Address, o => o.MapFrom(s => MappingHelpers.FormatAddress(s.Address)))
                // Filled by MemberService from the active membership.
                .ForMember(d => d.MembershipStartDate, o => o.Ignore())
                .ForMember(d => d.MembershipEndDate, o => o.Ignore());

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
        }
    }
}
