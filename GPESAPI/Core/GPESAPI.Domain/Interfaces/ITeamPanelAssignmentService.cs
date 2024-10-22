using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Domain.Interfaces
{
    public interface ITeamPanelAssignmentService
    {
        Task<IEnumerable<TeamPanelAssignment>> GetAllTeamPanelAssignmentsAsync();
        Task<TeamPanelAssignment> GetTeamPanelAssignmentByIdAsync(int id);
        Task AddTeamPanelAssignmentAsync(TeamPanelAssignment teamPanelAssignment);
        Task UpdateTeamPanelAssignmentAsync(TeamPanelAssignment teamPanelAssignment);
        Task DeleteTeamPanelAssignmentAsync(int id);
    }
}
