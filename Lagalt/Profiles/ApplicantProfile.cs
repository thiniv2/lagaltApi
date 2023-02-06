using AutoMapper;
using Lagalt.Models;
using Lagalt.Models.DTO;
using Lagalt.Relationships;
namespace Lagalt.Profiles
{
    public class ApplicantProfile : Profile
    {
        public ApplicantProfile()
        {
            CreateMap<Applicant, ApplicantDTO>();
            CreateMap<ApplicantDTO, Applicant>();
        }
    }
}
