using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface IProfessorAppService
    {
        Task<ProfessorDTO> AddProfessorAppAsync(ProfessorDTO professorDto);
        Task<List<ProfessorDTO>> GetAllProfessorAppAsync();
        Task<ProfessorDTO> GetByProfessorAppIdAsync(int id);
        Task<ProfessorDTO> GetByProfessorAppEmailAsync(string email);

        Task UpdateProfessorAppAsync(ProfessorDTO professorDto);
        Task DeleteProfessorAppAsync(int id);
    }
}
