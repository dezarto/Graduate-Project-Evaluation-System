using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface ITeamAppService
    {
        Task<IEnumerable<TeamDTO>> GetAllTeamAppAsync();
        Task<TeamDTO> GetTeamAppByIdAsync(int id);
        Task AddTeamAppAsync(TeamDTO teamDto);
        Task UpdateTeamAppAsync(TeamDTO teamDto);
        Task DeleteTeamAppAsync(int id);
        Task<IEnumerable<TeamDTO>> GetByAdvisorIdTeamAppAsync(int advisorId);
    }
}
