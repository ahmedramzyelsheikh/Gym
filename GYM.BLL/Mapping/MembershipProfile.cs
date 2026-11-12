using AutoMapper;
using GYM.BLL.ViewModels.Memberships;
using GYM.DAL.Data.Models;

namespace GYM.BLL.Mapping
{
    public class MembershipProfile : Profile
    {
        public MembershipProfile()
        {
            CreateMap<Membership, MemberShipViewModel>()
                .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member.Name))
                .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.CreateAt));
        }
    }
}
