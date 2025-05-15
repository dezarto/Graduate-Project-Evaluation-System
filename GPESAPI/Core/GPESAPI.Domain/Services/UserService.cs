using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUserAsync(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsByStudentNumberAsync(string studentNumber)
        {
            return await _userRepository.ExistsByStudentNumberAsync(studentNumber);
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetByUserIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task<List<ProfessorsUsers>> GetUsersWithProfessorsAsync()
        {
            var users = await _userRepository.GetAllUsersWithProfessorsAsync();

            return users.Select(user => new ProfessorsUsers
            {
                UserId = user.UserId,
                ProfessorId = user.ProfessorId
            }).ToList();
        }

        public async Task<User> GetByStudentNumberAsync(string studentNumber)
        {
            return await _userRepository.GetByStudentNumberAsync(studentNumber);
        }
    }
}
