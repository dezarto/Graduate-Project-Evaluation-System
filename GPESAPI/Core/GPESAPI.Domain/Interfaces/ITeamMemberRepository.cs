using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface ITeamMemberRepository
    {
        Task<IEnumerable<TeamMember>> GetAllTeamMembersAsync();
        Task AddTeamMemberAsync(TeamMember teamMember);
        Task UpdateTeamMemberAsync(TeamMember teamMember);
        Task DeleteTeamMemberAsync(int teamId);
        Task<TeamMember> GetByUserIdAsync(int userId);
        Task<List<TeamMember>> GetByTeamIdAsync(int teamId);
    }
}
