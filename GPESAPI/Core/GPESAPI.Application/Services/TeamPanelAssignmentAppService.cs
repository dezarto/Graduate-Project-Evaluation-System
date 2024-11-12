using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class TeamPanelAssignmentAppService : ITeamPanelAssignmentAppService
    {
        private readonly ITeamPanelAssignmentService _teamPanelAssignmentService;
        private readonly IMapper _mapper;

        public TeamPanelAssignmentAppService(ITeamPanelAssignmentService teamPanelAssignmentService, IMapper mapper)
        {
            _teamPanelAssignmentService = teamPanelAssignmentService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamPanelAssignmentDTO>> GetAllTeamPanelAssignmentAppAsync()
        {
            var assignments = await _teamPanelAssignmentService.GetAllTeamPanelAssignmentsAsync();
            return _mapper.Map<IEnumerable<TeamPanelAssignmentDTO>>(assignments);
        }

        public async Task<TeamPanelAssignmentDTO> GetTeamPanelAssignmentAppByIdAsync(int id)
        {
            var assignment = await _teamPanelAssignmentService.GetTeamPanelAssignmentByIdAsync(id);
            return _mapper.Map<TeamPanelAssignmentDTO>(assignment);
        }

        public async Task AddTeamPanelAssignmentAppAsync(TeamPanelAssignmentDTO assignmentDto)
        {
            var assignment = _mapper.Map<TeamPanelAssignment>(assignmentDto);
            await _teamPanelAssignmentService.AddTeamPanelAssignmentAsync(assignment);
        }

        public async Task UpdateTeamPanelAssignmentAppAsync(TeamPanelAssignmentDTO assignmentDto)
        {
            var assignment = _mapper.Map<TeamPanelAssignment>(assignmentDto);
            await _teamPanelAssignmentService.UpdateTeamPanelAssignmentAsync(assignment);
        }

        public async Task DeleteTeamPanelAssignmentAppAsync(int id)
        {
            await _teamPanelAssignmentService.DeleteTeamPanelAssignmentAsync(id);
        }
    }

}
