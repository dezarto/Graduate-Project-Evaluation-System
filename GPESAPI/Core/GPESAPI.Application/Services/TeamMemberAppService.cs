using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Application.Services
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

        public async Task DeleteTeamMemberAppAsync(int teamId)
        {
            await _teamMemberService.DeleteTeamMemberAsync(teamId);
        }

        public async Task<TeamMemberDTO> GetTeamMemberByUserIdAsync(int userId)
        {
            var teamMember = await _teamMemberService.GetByUserIdAsync(userId);
            if (teamMember != null)
            {
                return _mapper.Map<TeamMemberDTO>(teamMember);
            }
            return null;
        }

        public async Task<List<TeamMemberDTO>> GetTeamMemberByTeamIdAsync(int teamId)
        {
            var teamMembers = await _teamMemberService.GetByTeamIdAsync(teamId);
            if (teamMembers != null && teamMembers.Count > 0)
            {
                return _mapper.Map<List<TeamMemberDTO>>(teamMembers);
            }
            return null;
        }
    }
}
