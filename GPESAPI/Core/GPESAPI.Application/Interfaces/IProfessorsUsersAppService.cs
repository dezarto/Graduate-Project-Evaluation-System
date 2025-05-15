namespace GPESAPI.Application.Interfaces
{
    public interface IProfessorsUsersAppService
    {
        Task AddProfessorsUsersAppAsync(int professorId, int userId);
        Task<List<int>> GetUserIdsByProfessorIdAppAsync(int professorId);
        Task RemoveProfessorsUsersAppAsync(int professorId, int userId);
        Task<bool> ProfessorsUsersExistsAppAsync(int professorId, int userId);
    }
}
