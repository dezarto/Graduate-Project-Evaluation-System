using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Domain.Services
{
    public class ProjectSubmissionService : IProjectSubmissionService
    {
        private readonly IGenericRepository<ProjectSubmission> _projectSubmissionRepository;

        public ProjectSubmissionService(IGenericRepository<ProjectSubmission> projectSubmissionRepository)
        {
            _projectSubmissionRepository = projectSubmissionRepository;
        }

        public async Task<IEnumerable<ProjectSubmission>> GetAllProjectSubmissionsAsync()
        {
            return await _projectSubmissionRepository.GetAllAsync();
        }

        public async Task<ProjectSubmission> GetProjectSubmissionByIdAsync(int id)
        {
            return await _projectSubmissionRepository.GetByIdAsync(id);
        }

        public async Task AddProjectSubmissionAsync(ProjectSubmission projectSubmission)
        {
            await _projectSubmissionRepository.AddAsync(projectSubmission);
        }

        public async Task UpdateProjectSubmissionAsync(ProjectSubmission projectSubmission)
        {
            await _projectSubmissionRepository.UpdateAsync(projectSubmission);
        }

        public async Task DeleteProjectSubmissionAsync(int id)
        {
            await _projectSubmissionRepository.DeleteAsync(id);
        }
    }

}
