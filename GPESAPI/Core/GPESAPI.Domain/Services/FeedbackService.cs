using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Domain.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IGenericRepository<Feedback> _feedbackRepository;

        public FeedbackService(IGenericRepository<Feedback> feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _feedbackRepository.GetAllAsync();
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int id)
        {
            return await _feedbackRepository.GetByIdAsync(id);
        }

        public async Task AddFeedbackAsync(Feedback feedback)
        {
            await _feedbackRepository.AddAsync(feedback);
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            await _feedbackRepository.UpdateAsync(feedback);
        }

        public async Task DeleteFeedbackAsync(int id)
        {
            await _feedbackRepository.DeleteAsync(id);
        }
    }

}
