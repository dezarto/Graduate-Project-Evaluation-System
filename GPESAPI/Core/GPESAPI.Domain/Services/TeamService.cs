using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _teamRepository.GetAllTeamsAsync();
        }

        public async Task<Team> GetTeamByIdAsync(int id)
        {
            return await _teamRepository.GetTeamByIdAsync(id);
        }

        public async Task AddTeamAsync(Team team)
        {
            await _teamRepository.AddTeamAsync(team);

            team.TeamId = team.TeamId;
        }

        public async Task UpdateTeamAsync(Team team)
        {
            await _teamRepository.UpdateTeamAsync(team);
        }

        public async Task DeleteTeamAsync(int id)
        {
            await _teamRepository.DeleteTeamAsync(id);
        }

        public async Task<IEnumerable<Team>> GetByAdvisorIdTeamAsync(int advisorId)
        {
            return await _teamRepository.GetByAdvisorIdAsync(advisorId);
        }
    }

}
