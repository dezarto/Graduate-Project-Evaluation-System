using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Domain.Services
{
    public class TeamPanelAssignmentService : ITeamPanelAssignmentService
    {
        private readonly IGenericRepository<TeamPanelAssignment> _teamPanelAssignmentRepository;

        public TeamPanelAssignmentService(IGenericRepository<TeamPanelAssignment> teamPanelAssignmentRepository)
        {
            _teamPanelAssignmentRepository = teamPanelAssignmentRepository;
        }

        public async Task<IEnumerable<TeamPanelAssignment>> GetAllTeamPanelAssignmentsAsync()
        {
            return await _teamPanelAssignmentRepository.GetAllAsync();
        }

        public async Task<TeamPanelAssignment> GetTeamPanelAssignmentByIdAsync(int id)
        {
            return await _teamPanelAssignmentRepository.GetByIdAsync(id);
        }

        public async Task AddTeamPanelAssignmentAsync(TeamPanelAssignment teamPanelAssignment)
        {
            await _teamPanelAssignmentRepository.AddAsync(teamPanelAssignment);
        }

        public async Task UpdateTeamPanelAssignmentAsync(TeamPanelAssignment teamPanelAssignment)
        {
            await _teamPanelAssignmentRepository.UpdateAsync(teamPanelAssignment);
        }

        public async Task DeleteTeamPanelAssignmentAsync(int id)
        {
            await _teamPanelAssignmentRepository.DeleteAsync(id);
        }
    }

}
