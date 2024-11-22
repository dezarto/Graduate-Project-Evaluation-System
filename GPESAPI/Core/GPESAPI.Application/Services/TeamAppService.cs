using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class TeamAppService : ITeamAppService
    {
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public TeamAppService(ITeamService teamService, IMapper mapper)
        {
            _teamService = teamService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamDTO>> GetAllTeamAppAsync()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return _mapper.Map<IEnumerable<TeamDTO>>(teams);
        }

        public async Task<TeamDTO> GetTeamAppByIdAsync(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            return _mapper.Map<TeamDTO>(team);
        }

        public async Task AddTeamAppAsync(TeamDTO teamDto)
        {
            var team = _mapper.Map<Team>(teamDto);
            await _teamService.AddTeamAsync(team);

            teamDto.TeamId = team.TeamId;
        }

        public async Task UpdateTeamAppAsync(TeamDTO teamDto)
        {
            var team = _mapper.Map<Team>(teamDto);
            await _teamService.UpdateTeamAsync(team);
        }

        public async Task DeleteTeamAppAsync(int id)
        {
            await _teamService.DeleteTeamAsync(id);
        }

        public async Task<IEnumerable<TeamDTO>> GetByAdvisorIdTeamAppAsync(int advisorId)
        {
            var teams = await _teamService.GetByAdvisorIdTeamAsync(advisorId);
            return _mapper.Map<IEnumerable<TeamDTO>>(teams);
        }

    }
}
