using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface ITeamPresentationService
    {
        Task<bool> ValidatePresentationSlotAsync(DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task AddTeamPresentationAsync(TeamPresentation teamPresentation);
        Task<TeamPresentation> GetTeamPresentationByIdAsync(int id);
        Task<List<TeamPresentation>> GetAllTeamPresentationsAsync();
        Task UpdateTeamPresentationAsync(TeamPresentation teamPresentation);
        Task DeleteTeamPresentationAsync(int id);
        Task<List<TeamPresentation>> GetPresentationsByDateAsync(DateTime date);
    }
}
