using AutoMapper;
using GYM.BLL.ViewModels.Plans;
using GYM.DAL.Data.Models;

namespace GYM.BLL.Mapping.Plans
{
    public class PlanProfile : Profile
    {
        public PlanProfile()
        {
            CreateMap<Plan, PlanView>();
        }
    }
}
