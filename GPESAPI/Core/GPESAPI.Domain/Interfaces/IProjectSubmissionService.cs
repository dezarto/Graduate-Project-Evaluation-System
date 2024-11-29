using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IProjectSubmissionService
    {
        Task<IEnumerable<ProjectSubmission>> GetAllProjectSubmissionsAsync();
        Task<ProjectSubmission> GetProjectSubmissionByIdAsync(int id);
        Task AddProjectSubmissionAsync(ProjectSubmission projectSubmission);
        Task UpdateProjectSubmissionAsync(ProjectSubmission projectSubmission);
        Task DeleteProjectSubmissionAsync(int id);
    }
}
