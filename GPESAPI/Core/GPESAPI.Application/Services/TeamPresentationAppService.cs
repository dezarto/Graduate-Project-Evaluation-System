using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;

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

        public async Task<List<TeamPresentation>> GetAllTeamPresentationsAsync()
        {
            return await _teamPresentationService.GetAllTeamPresentationsAsync();
        }

        public async Task<TeamPresentation> GetTeamPresentationByIdAsync(int id)
        {
            return await _teamPresentationService.GetTeamPresentationByIdAsync(id);
        }

        public async Task AddTeamPresentationAsync(TeamPresentationDTO teamPresentationDto)
        {
            var teamPresentation = _mapper.Map<TeamPresentation>(teamPresentationDto);
            await _teamPresentationService.AddTeamPresentationAsync(teamPresentation);
        }

        public async Task UpdateTeamPresentationAsync(int id, TeamPresentationDTO teamPresentationDto)
        {
            var teamPresentation = new TeamPresentation
            {
                TeamPresentationId = id,
                TeamId = teamPresentationDto.TeamId,
                ProjectId = teamPresentationDto.ProjectId,
                AdvisorId = teamPresentationDto.AdvisorId,
                Professor1Id = teamPresentationDto.Professor1Id,
                Professor2Id = teamPresentationDto.Professor2Id,
                PresentationDate = teamPresentationDto.PresentationDate,
                StartTime = teamPresentationDto.StartTime,
                EndTime = teamPresentationDto.EndTime
            };

            await _teamPresentationService.UpdateTeamPresentationAsync(teamPresentation);
        }

        public async Task DeleteTeamPresentationAsync(int id)
        {
            await _teamPresentationService.DeleteTeamPresentationAsync(id);
        }

        public async Task SaveAllPresentationsAsync(List<TeamPresentation> presentations)
        {
            foreach (var presentation in presentations)
            {
                await _teamPresentationService.AddTeamPresentationAsync(presentation);
            }
        }
    }
}
