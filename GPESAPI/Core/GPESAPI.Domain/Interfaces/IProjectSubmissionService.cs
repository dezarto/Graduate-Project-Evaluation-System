using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Domain.Interfaces
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
