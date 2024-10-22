using GraduateProjectEvaluationSystemAPI.Application.DTOs;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface IProfessorAppService
    {
        Task<ProfessorDTO> AddProfessorAppAsync(ProfessorDTO professorDto);
        Task<List<ProfessorDTO>> GetAllProfessorAppAsync();
        Task<ProfessorDTO> GetByProfessorAppIdAsync(int id);
        Task UpdateProfessorAppAsync(ProfessorDTO professorDto);
        Task DeleteProfessorAppAsync(int id);
    }
}
