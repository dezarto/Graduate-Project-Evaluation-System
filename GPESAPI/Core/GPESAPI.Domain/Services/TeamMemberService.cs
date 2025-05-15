using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamMemberService(ITeamMemberRepository teamMemberRepository)
        {
            _teamMemberRepository = teamMemberRepository;
        }

        public async Task<IEnumerable<TeamMember>> GetAllTeamMembersAsync()
        {
            return await _teamMemberRepository.GetAllTeamMembersAsync();
        }

        public async Task AddTeamMemberAsync(TeamMember teamMember)
        {
            await _teamMemberRepository.AddTeamMemberAsync(teamMember);
        }

        public async Task UpdateTeamMemberAsync(TeamMember teamMember)
        {
            await _teamMemberRepository.UpdateTeamMemberAsync(teamMember);
        }

        public async Task DeleteTeamMemberAsync(int teamId)
        {
            await _teamMemberRepository.DeleteTeamMemberAsync(teamId);
        }

        public async Task<TeamMember> GetByUserIdAsync(int userId)
        {
            return await _teamMemberRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<TeamMember>> GetByTeamIdAsync(int teamId)
        {
            return await _teamMemberRepository.GetByTeamIdAsync(teamId);
        }
    }
}
