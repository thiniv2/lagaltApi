using AutoMapper;
using Lagalt.Models;
using Lagalt.Models.DTO;
using Lagalt.Relationships;
using Newtonsoft.Json.Linq;

namespace Lagalt.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDTO>().ForMember(p => p.Applicants, opt => opt.MapFrom(p => p.Applicants));
			CreateMap<ProjectDTO, Project>();

			CreateMap<Project, ProjectBannerDTO>()
                .ForMember(pbDto => pbDto.Field, opt => opt
                .MapFrom(p => Enum.GetName(typeof(FieldType), (int)p.Field)));

            CreateMap<ProjectBannerDTO, Project>();

			CreateMap<Project, ProjectDetailsDTO>();
			CreateMap<ProjectDetailsDTO, Project>();
            CreateMap<ProjectCreateDTO, Project>()
                .ForMember(p => p.TotalSkills, opt => opt
                .MapFrom(pcdto => pcdto.Skillset.Length))
           // CreateMap<ProjectCreateDTO, Project>()
                .ForMember(p => p.Owner, opt => opt
                .MapFrom(pcdto => pcdto.OwnerId));
		}
    }
}
