using GraduateProjectEvaluationSystemAPI.Application.DTOs;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface IProjectAppService
    {
        Task<IEnumerable<ProjectDTO>> GetAllProjectAppAsync();
        Task<ProjectDTO> GetProjectAppByIdAsync(int id);
        Task AddProjectAppAsync(ProjectDTO projectDto);
        Task UpdateProjectAppAsync(ProjectDTO projectDto);
        Task DeleteProjectAppAsync(int id);
    }
}
