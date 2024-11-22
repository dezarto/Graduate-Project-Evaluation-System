using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Professor, ProfessorDTO>().ReverseMap();
            CreateMap<ProfessorsUsers, ProfessorsUsersDTO>().ReverseMap(); 
            CreateMap<ProfessorAvailability, ProfessorAvailabilityDTO>().ReverseMap();
            CreateMap<Team, TeamDTO>().ReverseMap();
            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<TeamMember, TeamMemberDTO>().ReverseMap();
            CreateMap<TeamPresentation, TeamPresentationDTO>().ReverseMap();
        }
    }
}
