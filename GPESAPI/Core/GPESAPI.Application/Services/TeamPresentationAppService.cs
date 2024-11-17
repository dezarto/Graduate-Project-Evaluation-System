using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Application.Services
{
    public class TeamPresentationAppService : ITeamPresentationAppService
    {
        private readonly ITeamPresentationService _teamPresentationService;

        public TeamPresentationAppService(ITeamPresentationService teamPresentationService)
        {
            _teamPresentationService = teamPresentationService;
        }

        public async Task<List<TeamPresentation>> GetAllTeamPresentationsAsync()
        {
            return await _teamPresentationService.GetAllTeamPresentationsAsync();
        }

        public async Task<TeamPresentation> GetTeamPresentationByIdAsync(int id)
        {
            return await _teamPresentationService.GetTeamPresentationByIdAsync(id);
        }

        public async Task CreateTeamPresentationAsync(TeamPresentationDTO teamPresentationDto)
        {
            var teamPresentation = new TeamPresentation
            {
                TeamId = teamPresentationDto.TeamId,
                ProjectId = teamPresentationDto.ProjectId,
                AdvisorId = teamPresentationDto.AdvisorId,
                Professor1Id = teamPresentationDto.Professor1Id,
                Professor2Id = teamPresentationDto.Professor2Id,
                PresentationDate = teamPresentationDto.PresentationDate,
                StartTime = teamPresentationDto.StartTime,
                EndTime = teamPresentationDto.EndTime
            };
            await _teamPresentationService.AddTeamPresentationAsync(teamPresentation);
        }

        public async Task UpdateTeamPresentationAsync(int id, TeamPresentationDTO teamPresentationDto)
        {
            var teamPresentation = new TeamPresentation
            {
                Id = id,
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
