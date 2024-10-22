using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Domain.Interfaces
{
    public interface IProfessorsUsersRepository
    {
        Task AddAsync(ProfessorsUsers professorUser); 
        Task<List<int>> GetUserIdsByProfessorIdAsync(int professorId); 
        Task RemoveAsync(int professorId, int userId);
        Task<bool> ExistsAsync(int professorId, int userId);
    }
}
