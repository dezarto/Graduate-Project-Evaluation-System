using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class TeamMemberAppService : ITeamMemberAppService
    {
        private readonly ITeamMemberService _teamMemberService;
        private readonly IMapper _mapper;

        public TeamMemberAppService(ITeamMemberService teamMemberService, IMapper mapper)
        {
            _teamMemberService = teamMemberService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamMemberDTO>> GetAllTeamMemberAppAsync()
        {
            var members = await _teamMemberService.GetAllTeamMembersAsync();
            return _mapper.Map<IEnumerable<TeamMemberDTO>>(members);
        }

        public async Task<TeamMemberDTO> GetTeamMemberAppByIdAsync(int id)
        {
            var member = await _teamMemberService.GetTeamMemberByIdAsync(id);
            return _mapper.Map<TeamMemberDTO>(member);
        }

        public async Task AddTeamMemberAppAsync(TeamMemberDTO memberDto)
        {
            var member = _mapper.Map<TeamMember>(memberDto);
            await _teamMemberService.AddTeamMemberAsync(member);
        }

        public async Task UpdateTeamMemberAppAsync(TeamMemberDTO memberDto)
        {
            var member = _mapper.Map<TeamMember>(memberDto);
            await _teamMemberService.UpdateTeamMemberAsync(member);
        }

        public async Task DeleteTeamMemberAppAsync(int id)
        {
            await _teamMemberService.DeleteTeamMemberAsync(id);
        }
    }
}
