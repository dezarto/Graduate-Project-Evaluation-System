using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUserAsync(User user);
        Task<List<User>> GetAllUserAsync();
        Task<User> GetByUserIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<bool> ExistsByStudentNumberAsync(string studentNumber);
        Task<List<ProfessorsUsers>> GetUsersWithProfessorsAsync();
        Task<User> GetByStudentNumberAsync(string studentNumber);
    }
}
