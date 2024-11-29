using GPESAPI.Application.DTOs;

namespace GPESAPI.Application.Interfaces
{
    public interface ITeamPresentationAppService
    {
        Task<List<TeamPresentationDTO>> GetAllTeamPresentationsAsync();
        Task<List<TeamPresentationDTO>> GetTeamPresentationByIdAsync(int id);
        Task AddTeamPresentationAsync(TeamPresentationDTO teamPresentationDto);
        Task UpdateTeamPresentationAsync(int id, TeamPresentationDTO teamPresentationDto);
        Task DeleteTeamPresentationAsync(int id);
        Task SaveAllPresentationsAsync(List<TeamPresentationDTO> presentations);
    }
}
