using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Application.Services
{
    public class EvaluationCriteriaAppService : IEvaluationCriteriaAppService
    {
        private readonly IEvaluationCriteriaService _evaluationCriteriaService;
        private readonly IMapper _mapper;

        public EvaluationCriteriaAppService(IEvaluationCriteriaService evaluationCriteriaService, IMapper mapper)
        {
            _evaluationCriteriaService = evaluationCriteriaService;
            _mapper = mapper;
        }

        public async Task AddEvaluationCriteriaAsync(EvaluationCriteriaDTO evaluationCriteriaDto)
        {
            var evaluationCriteria = _mapper.Map<EvaluationCriteria>(evaluationCriteriaDto);

            await _evaluationCriteriaService.AddEvaluationCriteriaAsync(evaluationCriteria);

            evaluationCriteriaDto.CriteriaId = evaluationCriteria.CriteriaId;
        }

        public async Task DeleteEvaluationCriteriaAsync(int id)
        {
            var existingCriteria = await _evaluationCriteriaService.GetByEvaluationCriteriaIdAsync(id);
            if (existingCriteria == null)
                throw new ArgumentException($"Evaluation criteria with ID {id} not found.");

            await _evaluationCriteriaService.DeleteEvaluationCriteriaAsync(id);
        }

        public async Task<IEnumerable<EvaluationCriteriaDTO>> GetAllEvaluationCriteriaAsync()
        {
            var criteriaList = await _evaluationCriteriaService.GetAllEvaluationCriteriaAsync();

            return _mapper.Map<IEnumerable<EvaluationCriteriaDTO>>(criteriaList);
        }

        public async Task<EvaluationCriteriaDTO> GetByEvaluationCriteriaIdAsync(int id)
        {
            var criteria = await _evaluationCriteriaService.GetByEvaluationCriteriaIdAsync(id);
            if (criteria == null)
                throw new KeyNotFoundException($"Evaluation criteria with ID {id} not found.");

            return _mapper.Map<EvaluationCriteriaDTO>(criteria);
        }

        public async Task UpdateEvaluationCriteriaAsync(int id, EvaluationCriteriaDTO evaluationCriteriaDto)
        {
            var existingCriteria = await _evaluationCriteriaService.GetByEvaluationCriteriaIdAsync(id);
            if (existingCriteria == null)
                throw new ArgumentException($"Evaluation criteria with ID {id} not found.");

            var evaluationCriteria = _mapper.Map<EvaluationCriteria>(evaluationCriteriaDto);

            await _evaluationCriteriaService.UpdateEvaluationCriteriaAsync(evaluationCriteria);
        }
    }
}
