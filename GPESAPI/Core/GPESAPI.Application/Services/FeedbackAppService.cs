using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class FeedbackAppService : IFeedbackAppService
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;

        public FeedbackAppService(IFeedbackService feedbackService, IMapper mapper)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FeedbackDTO>> GetAllFeedbackAppAsync()
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            return _mapper.Map<IEnumerable<FeedbackDTO>>(feedbacks);
        }

        public async Task<FeedbackDTO> GetFeedbackAppByIdAsync(int id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            return _mapper.Map<FeedbackDTO>(feedback);
        }

        public async Task AddFeedbackAppAsync(FeedbackDTO feedbackDto)
        {
            var feedback = _mapper.Map<Feedback>(feedbackDto);
            await _feedbackService.AddFeedbackAsync(feedback);
        }

        public async Task UpdateFeedbackAppAsync(FeedbackDTO feedbackDto)
        {
            var feedback = _mapper.Map<Feedback>(feedbackDto);
            await _feedbackService.UpdateFeedbackAsync(feedback);
        }

        public async Task DeleteFeedbackAppAsync(int id)
        {
            await _feedbackService.DeleteFeedbackAsync(id);
        }
    }
}
