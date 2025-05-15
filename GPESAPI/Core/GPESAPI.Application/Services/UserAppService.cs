using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Entities;

namespace GPESAPI.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserAppService(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserDTO> AddUserAppAsync(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var addedUser = await _userService.AddUserAsync(user);
            return _mapper.Map<UserDTO>(addedUser);
        }

        public async Task DeleteUserAppAsync(int id)
        {
            await _userService.DeleteUserAsync(id);
        }

        public async Task<bool> ExistsByStudentNumberAppAsync(string studentNumber)
        {
            return await _userService.ExistsByStudentNumberAsync(studentNumber);
        }

        public async Task<List<UserDTO>> GetAllUserAppAsync()
        {
            var users = await _userService.GetAllUserAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetByStudentNumberAsync(string studentNumber)
        {
            var user = await _userService.GetByStudentNumberAsync(studentNumber);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetByUserAppIdAsync(int id)
        {
            var user = await _userService.GetByUserIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<ProfessorsUsersDTO>> GetUsersWithProfessorsAppAsync()
        {
            var professorsUsers = await _userService.GetUsersWithProfessorsAsync();

            return _mapper.Map<List<ProfessorsUsersDTO>>(professorsUsers);
        }

        public async Task UpdateUserAppAsync(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _userService.UpdateUserAsync(user);
        }
    }
}
