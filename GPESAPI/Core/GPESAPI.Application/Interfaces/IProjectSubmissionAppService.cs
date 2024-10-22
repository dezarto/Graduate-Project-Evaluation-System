using GraduateProjectEvaluationSystemAPI.Application.DTOs;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface IProjectSubmissionAppService
    {
        Task<IEnumerable<ProjectSubmissionDTO>> GetAllProjectSubmissionAppAsync();
        Task<ProjectSubmissionDTO> GetProjectSubmissionAppByIdAsync(int id);
        Task AddProjectSubmissionAppAsync(ProjectSubmissionDTO projectSubmissionDto);
        Task UpdateProjectSubmissionAppAsync(ProjectSubmissionDTO projectSubmissionDto);
        Task DeleteProjectSubmissionAppAsync(int id);
    }
}
