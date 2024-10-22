using GraduateProjectEvaluationSystemAPI.Application.DTOs;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface IUserAppService
    {
        Task<UserDTO> AddUserAppAsync(UserDTO userDto);
        Task<List<UserDTO>> GetAllUserAppAsync();
        Task<UserDTO> GetByUserAppIdAsync(int id);
        Task UpdateUserAppAsync(UserDTO userDto);
        Task DeleteUserAppAsync(int id);
        Task<bool> ExistsByStudentNumberAppAsync(string studentNumber);
        Task<List<ProfessorsUsersDTO>> GetUsersWithProfessorsAppAsync();
    }
}
