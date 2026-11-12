using AutoMapper;
using GYM.BLL.ViewModels.Trainers;
using GYM.DAL.Data.Models;

namespace GYM.BLL.Mapping
{
    public class TrainerProfile : Profile
    {
        public TrainerProfile()
        {
            CreateMap<Trainer, TrainerView>()
                .ForMember(d => d.Specialties, o => o.MapFrom(s => s.Specialtiy.ToString()))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToString("dd MMM yyyy")))
                .ForMember(d => d.Address, o => o.MapFrom(s => MappingHelpers.FormatAddress(s.Address)));

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
        }
    }
}
