using GraduateProjectEvaluationSystemAPI.Application.DTOs;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface ITeamPanelAssignmentAppService
    {
        Task<IEnumerable<TeamPanelAssignmentDTO>> GetAllTeamPanelAssignmentAppAsync();
        Task<TeamPanelAssignmentDTO> GetTeamPanelAssignmentAppByIdAsync(int id);
        Task AddTeamPanelAssignmentAppAsync(TeamPanelAssignmentDTO teamPanelAssignmentDto);
        Task UpdateTeamPanelAssignmentAppAsync(TeamPanelAssignmentDTO teamPanelAssignmentDto);
        Task DeleteTeamPanelAssignmentAppAsync(int id);
    }
}
