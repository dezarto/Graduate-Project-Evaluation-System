using GraduateProjectEvaluationSystemAPI.Application.DTOs;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface ITeamMemberAppService
    {
        Task<IEnumerable<TeamMemberDTO>> GetAllTeamMemberAppAsync();
        Task<TeamMemberDTO> GetTeamMemberAppByIdAsync(int id);
        Task AddTeamMemberAppAsync(TeamMemberDTO teamMemberDto);
        Task UpdateTeamMemberAppAsync(TeamMemberDTO teamMemberDto);
        Task DeleteTeamMemberAppAsync(int id);
    }
}
