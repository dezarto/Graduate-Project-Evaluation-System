using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Domain.Services
{
    public class TeamPresentationService : ITeamPresentationService
    {
        private readonly ITeamPresentationRepository _repository;

        public TeamPresentationService(ITeamPresentationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ValidatePresentationSlotAsync(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return !await _repository.IsPresentationSlotTakenAsync(date, startTime, endTime);
        }

        public async Task AddTeamPresentationAsync(TeamPresentation teamPresentation)
        {
            await _repository.AddTeamPresentationAsync(teamPresentation);
        }

        public async Task<List<TeamPresentation>> GetTeamPresentationByIdAsync(int id)
        {
            var presentation = await _repository.GetTeamPresentationByIdAsync(id);
            
            return presentation;
        }

        public async Task<List<TeamPresentation>> GetAllTeamPresentationsAsync()
        {
            return await _repository.GetAllTeamPresentationsAsync();
        }

        public async Task UpdateTeamPresentationAsync(TeamPresentation teamPresentation)
        {
            var existingPresentation = await _repository.GetTeamPresentationByIdAsync(teamPresentation.TeamPresentationId);
            if (existingPresentation == null)
            {
                throw new Exception("TeamPresentation not found.");
            }

            bool isSlotAvailable = await ValidatePresentationSlotAsync(
                teamPresentation.PresentationDate,
                teamPresentation.StartTime,
                teamPresentation.EndTime);

            if (!isSlotAvailable)
            {
                throw new Exception("The presentation slot is already taken.");
            }

            await _repository.UpdateTeamPresentationAsync(teamPresentation);
        }

        public async Task DeleteTeamPresentationAsync(int id)
        {
            var presentation = await _repository.GetTeamPresentationByIdAsync(id);
            if (presentation == null)
            {
                throw new Exception("TeamPresentation not found.");
            }

            await _repository.DeleteTeamPresentationAsync(id);
        }

        public async Task<List<TeamPresentation>> GetPresentationsByDateAsync(DateTime date)
        {
            
            var presentations = await _repository.GetPresentationsByDateAsync(date);

            return presentations;
        }

        public async Task<TeamPresentation> GetTeamPresentationByTeamIdAsync(int id)
        {
            var presentation = await _repository.GetTeamPresentationByTeamIdAsync(id);

            return presentation;
        }
    }
}
