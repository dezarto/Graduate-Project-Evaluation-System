using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Application.Services
{
    public class ProfessorsUsersAppService : IProfessorsUsersAppService
    {
        private readonly IProfessorsUsersService _professorsUsersService;

        public ProfessorsUsersAppService(IProfessorsUsersService professorsUsersService)
        {
            _professorsUsersService = professorsUsersService;
        }

        public async Task AddProfessorsUsersAppAsync(int professorId, int userId)
        {
            await _professorsUsersService.AddProfessorsUsersAsync(professorId, userId);
        }

        public async Task<List<int>> GetUserIdsByProfessorIdAppAsync(int professorId)
        {
            return await _professorsUsersService.GetUserIdsByProfessorIdAsync(professorId);
        }

        public async Task RemoveProfessorsUsersAppAsync(int professorId, int userId)
        {
            await _professorsUsersService.RemoveProfessorsUsersAsync(professorId, userId);
        }

        public async Task<bool> ProfessorsUsersExistsAppAsync(int professorId, int userId)
        {
            return await _professorsUsersService.ProfessorsUsersExistsAsync(professorId, userId);
        }
    }
}
