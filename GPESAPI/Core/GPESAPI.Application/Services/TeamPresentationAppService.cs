using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Application.Services
{
    public class TeamPresentationAppService : ITeamPresentationAppService
    {
        private readonly ITeamPresentationService _teamPresentationService;
        private readonly IMapper _mapper;

        public TeamPresentationAppService(ITeamPresentationService teamPresentationService, IMapper mapper)
        {
            _teamPresentationService = teamPresentationService;
            _mapper = mapper;
        }

        public async Task<List<TeamPresentationDTO>> GetAllTeamPresentationsAsync()
        {
            var allTeamPresentations = await _teamPresentationService.GetAllTeamPresentationsAsync();
            return _mapper.Map<List<TeamPresentationDTO>>(allTeamPresentations);
        }

        public async Task<List<TeamPresentationDTO>> GetTeamPresentationByIdAsync(int id)
        {
            var teamPresentations = await _teamPresentationService.GetTeamPresentationByIdAsync(id);
            return _mapper.Map<List<TeamPresentationDTO>>(teamPresentations);
        }

        public async Task AddTeamPresentationAsync(TeamPresentationDTO teamPresentationDto)
        {
            var teamPresentation = _mapper.Map<TeamPresentation>(teamPresentationDto);
            await _teamPresentationService.AddTeamPresentationAsync(teamPresentation);
        }

        public async Task UpdateTeamPresentationAsync(int id, TeamPresentationDTO teamPresentationDto)
        {
            var teamPresentation = _mapper.Map<TeamPresentation>(teamPresentationDto);
            teamPresentation.TeamPresentationId = id;
            await _teamPresentationService.UpdateTeamPresentationAsync(teamPresentation);
        }

        public async Task DeleteTeamPresentationAsync(int id)
        {
            await _teamPresentationService.DeleteTeamPresentationAsync(id);
        }

        public async Task SaveAllPresentationsAsync(List<TeamPresentationDTO> presentations)
        {
            var teamPresentations = _mapper.Map<List<TeamPresentation>>(presentations);
            foreach (var presentation in teamPresentations)
            {
                await _teamPresentationService.AddTeamPresentationAsync(presentation);
            }
        }
    }
}
