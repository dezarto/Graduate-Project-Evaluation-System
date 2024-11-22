using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Domain.Interfaces
{
    public interface IProfessorsUsersService
    {
        Task AddProfessorsUsersAsync(int professorId, int userId);
        Task<List<int>> GetUserIdsByProfessorIdAsync(int professorId);
        Task RemoveProfessorsUsersAsync(int professorId, int userId);
        Task<bool> ProfessorsUsersExistsAsync(int professorId, int userId);
    }
}
