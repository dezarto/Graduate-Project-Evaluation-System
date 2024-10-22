using GraduateProjectEvaluationSystemAPI.Application.DTOs;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface ITeamAppService
    {
        Task<IEnumerable<TeamDTO>> GetAllTeamAppAsync();
        Task<TeamDTO> GetTeamAppByIdAsync(int id);
        Task AddTeamAppAsync(TeamDTO teamDto);
        Task UpdateTeamAppAsync(TeamDTO teamDto);
        Task DeleteTeamAppAsync(int id);
    }
}
