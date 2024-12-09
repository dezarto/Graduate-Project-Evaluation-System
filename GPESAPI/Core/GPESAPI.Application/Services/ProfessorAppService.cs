using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Entities;

namespace GPESAPI.Application.Services
{
    public class ProfessorAppService : IProfessorAppService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly IProjectService _projectService;
        private readonly IProfessorService _professorService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly IProfessorsUsersService _professorsUsersService;
        private readonly ITeamPresentationService _teamPresentationService;
        
        public ProfessorAppService(IProfessorService professorService, IMapper mapper, IProfessorsUsersService professorsUsersService, IUserService userService, ITeamService teamService, ITeamPresentationService teamPresentationService, IProjectService projectService, ITeamMemberService teamMemberService)
        {
            _professorService = professorService;
            _professorsUsersService = professorsUsersService;
            _userService = userService;
            _teamService = teamService;
            _teamPresentationService = teamPresentationService;
            _professorService = professorService;
            _projectService = projectService;
            _teamMemberService = teamMemberService;
            _mapper = mapper;
        }

        public async Task<List<ProfessorDTO>> GetAllProfessorAppAsync()
        {
            var professors = await _professorService.GetAllProfessorAsync();
            return _mapper.Map<List<ProfessorDTO>>(professors);
        }

        public async Task<ProfessorDTO> GetByProfessorAppIdAsync(int id)
        {
            var professor = await _professorService.GetByProfessorIdAsync(id);
            return _mapper.Map<ProfessorDTO>(professor);
        }

        public async Task<ProfessorDTO> AddProfessorAppAsync(ProfessorDTO professorDto)
        {
            var professor = _mapper.Map<Professor>(professorDto);
            var addedProfessor = await _professorService.AddProfessorAsync(professor);
            return _mapper.Map<ProfessorDTO>(addedProfessor);
        }

        public async Task UpdateProfessorAppAsync(ProfessorDTO professorDto)
        {
            var professor = _mapper.Map<Professor>(professorDto);
            await _professorService.UpdateProfessorAsync(professor);
        }

        public async Task DeleteProfessorAppAsync(int id)
        {
            await _professorService.DeleteProfessorAsync(id);
        }

        public async Task<ProfessorDTO> GetByProfessorAppEmailAsync(string email)
        {
            var professor = await _professorService.GetByProfessorEmailAsync(email);
            return _mapper.Map<ProfessorDTO>(professor);
        }

        

        public async Task<string> ProfessorApprovalTeams(int teamId, bool approval)
        {
            var teamInfo = await _teamService.GetTeamByIdAsync(teamId);

            if (teamInfo == null)
            {
                throw new Exception("No teams found.");
            }

            if (approval)
            {
                teamInfo.isActive = true;
                await _teamService.UpdateTeamAsync(teamInfo);
                return "Team approved.";
            }
            else
            {
                await _teamMemberService.DeleteTeamMemberAsync(teamInfo.TeamId);
                await _teamService.DeleteTeamAsync(teamId);
                await _projectService.DeleteProjectAsync(teamInfo.ProjectId);
                return "The team was not approved and was deleted.";
            }
        }

        public async Task<List<TeamDTO>> ProfessorApprovalTeamsView(string professorMail)
        {
            var finProfessor = await _professorService.GetByProfessorEmailAsync(professorMail);

            if (finProfessor == null)
            {
                throw new Exception("Resource not found.");
            }

            var teamInfos = await _teamService.GetByAdvisorIdTeamAsync(finProfessor.ProfessorId);

            if (teamInfos == null || !teamInfos.Any())
            {
                throw new Exception("No teams found.");
            }

            var enrichedTeamInfos = teamInfos.Select(team => new TeamDTO
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                ProjectId = team.ProjectId,
                AdvisorId = team.AdvisorId,
                isActive = team.isActive
            }).ToList();

            return enrichedTeamInfos;
        }
    }
}
