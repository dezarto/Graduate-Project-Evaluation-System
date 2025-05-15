using GPESAPI.Application.DTOs;
using GPESAPI.Domain.Entities;

namespace GPESAPI.Application.Interfaces
{
    public interface IProjectAppService
    {
        Task<IEnumerable<ProjectDTO>> GetAllProjectAppAsync();
        Task<ProjectDTO> GetProjectAppByIdAsync(int id);
        Task AddProjectAppAsync(ProjectDTO projectDto);
        Task UpdateProjectAppAsync(ProjectDTO projectDto);
        Task DeleteProjectAppAsync(int id);
        Task<StudentProjectTeams> StudentProjectTeamView(string studentNumber);
        Task<List<ProjectTeams>> ProfessorProjectTeamView(string professorMail);
        Task<ProjectTeamResult> ProfessorProjectTeamResult(string professorMail, int teamId);
    }
}
