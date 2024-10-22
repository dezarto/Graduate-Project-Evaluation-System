using GraduateProjectEvaluationSystemAPI.Application.DTOs;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface IFeedbackAppService
    {
        Task<IEnumerable<FeedbackDTO>> GetAllFeedbackAppAsync();
        Task<FeedbackDTO> GetFeedbackAppByIdAsync(int id);
        Task AddFeedbackAppAsync(FeedbackDTO feedbackDto);
        Task UpdateFeedbackAppAsync(FeedbackDTO feedbackDto);
        Task DeleteFeedbackAppAsync(int id);
    }
}
