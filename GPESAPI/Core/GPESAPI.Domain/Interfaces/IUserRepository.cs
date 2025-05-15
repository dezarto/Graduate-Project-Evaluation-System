using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<bool> ExistsByStudentNumberAsync(string studentNumber);
        Task<List<User>> GetAllUsersWithProfessorsAsync();
        Task<User> GetByStudentNumberAsync(string studentNumber);
    }
}
