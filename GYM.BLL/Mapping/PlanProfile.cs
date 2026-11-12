using AutoMapper;
using GYM.BLL.ViewModels;
using GYM.DAL.Data.Models;

namespace GYM.BLL.Mapping
{
    public class PlanProfile : Profile
    {
        public PlanProfile()
        {
            CreateMap<Plan, PlanView>();
        }
    }
}
