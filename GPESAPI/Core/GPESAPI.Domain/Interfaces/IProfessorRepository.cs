using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IProfessorRepository
    {
        Task<Professor> AddAsync(Professor professor);
        Task<List<Professor>> GetAllAsync();
        Task<Professor> GetByIdAsync(int id);
        Task<Professor> GetByEmailAsync(string email);
        Task UpdateAsync(Professor professor);
        Task DeleteAsync(int id);
    }
}
