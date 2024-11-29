using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface ITeamPresentationRepository
    {
        Task AddTeamPresentationAsync(TeamPresentation teamPresentation);
        Task<List<TeamPresentation>> GetTeamPresentationByIdAsync(int id);
        Task<List<TeamPresentation>> GetAllTeamPresentationsAsync();
        Task UpdateTeamPresentationAsync(TeamPresentation teamPresentation);
        Task DeleteTeamPresentationAsync(int id);
        Task<List<TeamPresentation>> GetPresentationsByDateAsync(DateTime date);
        Task<bool> IsPresentationSlotTakenAsync(DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task<TeamPresentation> GetTeamPresentationByTeamIdAsync(int id);
    }
}
