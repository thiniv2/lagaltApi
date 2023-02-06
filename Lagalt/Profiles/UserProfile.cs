using AutoMapper;
using Lagalt.Models;
using Lagalt.Models.DTO;
using Lagalt.Relationships;
namespace Lagalt.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDTO>()
                .ForMember(udto => udto.Projects, opt => opt
                .MapFrom(u => u.Projects.Select(u => u.Id).ToArray()));
            CreateMap<User, UserReadDTO>()
                .ForMember(udto => udto.CollaborationProjects, opt => opt
                .MapFrom(u => u.CollaborationProjects.Select(u => u.Id).ToArray()));
            CreateMap<UserEditDTO, User>();
            CreateMap<UserCreateDTO, User>()
                .ForMember(user => user.Id, opt => opt
                .MapFrom(ucdto => ucdto.MicrosoftId));
        }
    }
}
