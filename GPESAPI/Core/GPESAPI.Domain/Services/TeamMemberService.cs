using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Domain.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly IGenericRepository<TeamMember> _teamMemberRepository;

        public TeamMemberService(IGenericRepository<TeamMember> teamMemberRepository)
        {
            _teamMemberRepository = teamMemberRepository;
        }

        public async Task<IEnumerable<TeamMember>> GetAllTeamMembersAsync()
        {
            return await _teamMemberRepository.GetAllAsync();
        }

        public async Task<TeamMember> GetTeamMemberByIdAsync(int id)
        {
            return await _teamMemberRepository.GetByIdAsync(id);
        }

        public async Task AddTeamMemberAsync(TeamMember teamMember)
        {
            await _teamMemberRepository.AddAsync(teamMember);
        }

        public async Task UpdateTeamMemberAsync(TeamMember teamMember)
        {
            await _teamMemberRepository.UpdateAsync(teamMember);
        }

        public async Task DeleteTeamMemberAsync(int id)
        {
            await _teamMemberRepository.DeleteAsync(id);
        }
    }
}
