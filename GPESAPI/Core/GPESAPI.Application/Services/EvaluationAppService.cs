using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Services;

namespace GPESAPI.Application.Services
{
    public class EvaluationAppService : IEvaluationAppService
    {
        private readonly IEvaluationService _evaluationService;
        private readonly IProfessorService _professorService;
        private readonly IEvaluationCriteriaDetailService _evaluationCriteriaDetailService;
        private readonly IChecklistItemDetailService _checklistItemDetailService;
        private readonly IEvaluationCriteriaAppService _evaluationCriteriaAppService;
        private readonly IChecklistItemsAppService _checklistItemsAppService;
        private readonly IMapper _mapper;

        public EvaluationAppService(IEvaluationService evaluationService, IProfessorService professorService, IEvaluationCriteriaDetailService evaluationCriteriaDetailService, IChecklistItemDetailService checklistItemDetailService, IEvaluationCriteriaAppService evaluationCriteriaAppService, IChecklistItemsAppService checklistItemsAppService, IMapper mapper)
        {
            _evaluationService = evaluationService;
            _professorService = professorService;
            _evaluationCriteriaDetailService = evaluationCriteriaDetailService;
            _checklistItemDetailService = checklistItemDetailService;
            _evaluationCriteriaAppService = evaluationCriteriaAppService;
            _checklistItemsAppService = checklistItemsAppService;
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

        public async Task<bool> SubmitEvaluationSave(EvaluateReasult evaluateResult, string professorMail)
        {
            var professor = await _professorService.GetByProfessorEmailAsync(professorMail);

            if (evaluateResult == null)
                throw new ArgumentNullException(nameof(evaluateResult));

            var alreadyEvaluated = await _evaluationService.HasMatchingProfessorAndTeamAsync(professor.ProfessorId, evaluateResult.TeamId);

            if (!alreadyEvaluated)
            {
                var evaluation = new Evaluation
                {
                    TeamId = evaluateResult.TeamId,
                    GeneralComments = evaluateResult.GeneralComments,
                    EvaluationScore = evaluateResult.TotalScore,
                    ProfessorId = professor.ProfessorId,
                    Date = evaluateResult.Date,
                };

                await _evaluationService.AddEvaluationAsync(evaluation);

                foreach (var criteria in evaluateResult.EvaluationCriterias)
                {
                    var criteriaDetail = new EvaluationCriteriaDetail
                    {
                        EvaluationId = evaluation.EvaluationId,
                        CriteriaId = criteria.CriteriaId,
                        isChecked = criteria.isChecked,
                        Score = criteria.Score,
                        Feedback = criteria.Feedback,
                    };

                    await _evaluationCriteriaDetailService.AddEvaluationCriteriaDetailAsync(criteriaDetail);
                }

                foreach (var checklistItem in evaluateResult.EvaluationChecklistItems)
                {
                    var checklistItemDetail = new ChecklistItemDetail
                    {
                        EvaluationId = evaluation.EvaluationId,
                        ItemId = checklistItem.ItemId,
                        isChecked = checklistItem.isChecked,
                        Feedback = checklistItem.Feedback
                    };

                    await _checklistItemDetailService.AddChecklistItemDetailAsync(checklistItemDetail);
                }

                return true;
            }
            return false;
        }

        public async Task<EvaluateReasult> GetEvaluationResult(int evaluateId)
        {
            var evaluation = await _evaluationService.GetEvaluationByIdAsync(evaluateId);
            
            if (evaluation == null)
                throw new KeyNotFoundException($"Evaluation with ID {evaluateId} not found.");

            var evaluationCriteriaDetails = await _evaluationCriteriaDetailService.GetEvaluationCriteriaDetailByIdAsync(evaluateId);
            
            var checklistItemDetails = await _checklistItemDetailService.GetByChecklistItemDetailIdAsync(evaluateId);

            var evaluationResult = new EvaluateReasult
            {
                EvaluationId = evaluateId,
                TeamId = evaluation.TeamId,
                GeneralComments = evaluation.GeneralComments,
                TotalScore = evaluation.EvaluationScore,
                Date = evaluation.Date,
                EvaluationChecklistItems = checklistItemDetails.Select(item => new EvaluationChecklistItemResult
                {
                    ItemId = item.ItemId,
                    isChecked = item.isChecked,
                    Feedback = item.Feedback
                }).ToList(),
                EvaluationCriterias = evaluationCriteriaDetails.Select(criterion => new EvaluationCriteriaResult
                {
                    CriteriaId = criterion.CriteriaId,
                    isChecked = criterion.isChecked,
                    Score = criterion.Score,
                    Feedback = criterion.Feedback
                }).ToList(),
            };

            return evaluationResult;
        }

        public async Task<AllCriterias> GetAllCriterias()
        {
            var criteria = (await _evaluationCriteriaAppService.GetAllEvaluationCriteriaAsync()).ToList();
            var items = (await _checklistItemsAppService.GetAllChecklistItemsAsync()).ToList();

            var newAllCriterias = new AllCriterias
            {
                ChecklistItemDatas = items,
                EvaluationCriteriaDatas = criteria
            };

            return newAllCriterias;
        }
    }
}
