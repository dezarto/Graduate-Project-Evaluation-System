using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Domain.Interfaces
{
    public interface IProfessorRepository
    {
        Task<Professor> AddAsync(Professor professor);
        Task<List<Professor>> GetAllAsync();
        Task<Professor> GetByIdAsync(int id);
        Task UpdateAsync(Professor professor);
        Task DeleteAsync(int id);
    }
}
