using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class ProjectSubmissionAppService : IProjectSubmissionAppService
    {
        private readonly IProjectSubmissionService _projectSubmissionService;
        private readonly IMapper _mapper;

        public ProjectSubmissionAppService(IProjectSubmissionService projectSubmissionService, IMapper mapper)
        {
            _projectSubmissionService = projectSubmissionService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectSubmissionDTO>> GetAllProjectSubmissionAppAsync()
        {
            var submissions = await _projectSubmissionService.GetAllProjectSubmissionsAsync();
            return _mapper.Map<IEnumerable<ProjectSubmissionDTO>>(submissions);
        }

        public async Task<ProjectSubmissionDTO> GetProjectSubmissionAppByIdAsync(int id)
        {
            var submission = await _projectSubmissionService.GetProjectSubmissionByIdAsync(id);
            return _mapper.Map<ProjectSubmissionDTO>(submission);
        }

        public async Task AddProjectSubmissionAppAsync(ProjectSubmissionDTO submissionDto)
        {
            var submission = _mapper.Map<ProjectSubmission>(submissionDto);
            await _projectSubmissionService.AddProjectSubmissionAsync(submission);
        }

        public async Task UpdateProjectSubmissionAppAsync(ProjectSubmissionDTO submissionDto)
        {
            var submission = _mapper.Map<ProjectSubmission>(submissionDto);
            await _projectSubmissionService.UpdateProjectSubmissionAsync(submission);
        }

        public async Task DeleteProjectSubmissionAppAsync(int id)
        {
            await _projectSubmissionService.DeleteProjectSubmissionAsync(id);
        }
    }
}
