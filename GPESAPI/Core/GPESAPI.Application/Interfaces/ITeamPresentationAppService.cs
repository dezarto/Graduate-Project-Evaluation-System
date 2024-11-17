using GPESAPI.Application.DTOs;
using GPESAPI.Domain.Entities;

namespace GPESAPI.Application.Interfaces
{
    public interface ITeamPresentationAppService
    {
        Task<List<TeamPresentation>> GetAllTeamPresentationsAsync();
        Task<TeamPresentation> GetTeamPresentationByIdAsync(int id);
        Task CreateTeamPresentationAsync(TeamPresentationDTO teamPresentationDto);
        Task UpdateTeamPresentationAsync(int id, TeamPresentationDTO teamPresentationDto);
        Task DeleteTeamPresentationAsync(int id);
        Task SaveAllPresentationsAsync(List<TeamPresentation> presentations);
    }
}
