using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class EvaluationAppService : IEvaluationAppService
    {
        private readonly IEvaluationService _evaluationService;
        private readonly IMapper _mapper;

        public EvaluationAppService(IEvaluationService evaluationService, IMapper mapper)
        {
            _evaluationService = evaluationService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EvaluationDTO>> GetAllEvaluationAppAsync()
        {
            var evaluations = await _evaluationService.GetAllEvaluationAsync();
            return _mapper.Map<IEnumerable<EvaluationDTO>>(evaluations);
        }

        public async Task<EvaluationDTO> GetEvaluationAppByIdAsync(int id)
        {
            var evaluation = await _evaluationService.GetEvaluationByIdAsync(id);
            return _mapper.Map<EvaluationDTO>(evaluation);
        }

        public async Task AddEvaluationAppAsync(EvaluationDTO evaluationDto)
        {
            var evaluation = _mapper.Map<Evaluation>(evaluationDto);
            await _evaluationService.AddEvaluationAsync(evaluation);
        }

        public async Task UpdateEvaluationAppAsync(EvaluationDTO evaluationDto)
        {
            var evaluation = _mapper.Map<Evaluation>(evaluationDto);
            await _evaluationService.UpdateEvaluationAsync(evaluation);
        }

        public async Task DeleteEvaluationAppAsync(int id)
        {
            await _evaluationService.DeleteEvaluationAsync(id);
        }
    }
}
