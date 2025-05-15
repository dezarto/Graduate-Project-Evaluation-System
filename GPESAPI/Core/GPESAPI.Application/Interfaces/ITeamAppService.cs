using GPESAPI.Application.DTOs;

namespace GPESAPI.Application.Interfaces
{
    public interface ITeamAppService
    {
        Task<object> CreateTeamAsync(string username, TeamCreator teamCreator);
        Task<IEnumerable<TeamDTO>> GetAllTeamAppAsync();
        Task<TeamDTO> GetTeamAppByIdAsync(int id);
        Task AddTeamAppAsync(TeamDTO teamDto);
        Task UpdateTeamAppAsync(TeamDTO teamDto);
        Task DeleteTeamAppAsync(int id);
        Task<IEnumerable<TeamDTO>> GetByAdvisorIdTeamAppAsync(int advisorId);
    }
}
