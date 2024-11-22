using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Domain.Interfaces
{
    public interface IProfessorService
    {
        Task<Professor> AddProfessorAsync(Professor professor);
        Task<List<Professor>> GetAllProfessorAsync();
        Task<Professor> GetByProfessorIdAsync(int id);
        Task<Professor> GetByProfessorEmailAsync(string email);
        Task UpdateProfessorAsync(Professor professor);
        Task DeleteProfessorAsync(int id);
    }
}
