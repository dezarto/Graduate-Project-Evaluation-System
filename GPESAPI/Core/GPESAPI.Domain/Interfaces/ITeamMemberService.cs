using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Domain.Interfaces
{
    public interface ITeamMemberService
    {
        Task<IEnumerable<TeamMember>> GetAllTeamMembersAsync();
        Task<TeamMember> GetTeamMemberByIdAsync(int id);
        Task AddTeamMemberAsync(TeamMember teamMember);
        Task UpdateTeamMemberAsync(TeamMember teamMember);
        Task DeleteTeamMemberAsync(int id);
    }
}
