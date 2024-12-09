using GPESAPI.Application.DTOs;

namespace GPESAPI.Application.Interfaces
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
