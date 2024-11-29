using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface ITeamMemberService
    {
        Task<IEnumerable<TeamMember>> GetAllTeamMembersAsync();
        Task<TeamMember> GetByUserIdAsync(int userId);
        Task<List<TeamMember>> GetByTeamIdAsync(int teamId);
        Task AddTeamMemberAsync(TeamMember teamMember);
        Task UpdateTeamMemberAsync(TeamMember teamMember);
        Task DeleteTeamMemberAsync(int teamId);
    }
}
