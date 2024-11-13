using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Domain.Services
{
    public class TeamService : ITeamService
    {
        private readonly IGenericRepository<Team> _teamRepository;

        public TeamService(IGenericRepository<Team> teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _teamRepository.GetAllAsync();
        }

        public async Task<Team> GetTeamByIdAsync(int id)
        {
            return await _teamRepository.GetByIdAsync(id);
        }

        public async Task AddTeamAsync(Team team)
        {
            await _teamRepository.AddAsync(team);

            team.TeamId = team.TeamId;
        }

        public async Task UpdateTeamAsync(Team team)
        {
            await _teamRepository.UpdateAsync(team);
        }

        public async Task DeleteTeamAsync(int id)
        {
            await _teamRepository.DeleteAsync(id);
        }
    }

}
