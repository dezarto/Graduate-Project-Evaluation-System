using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Domain.Services
{
    public class ProfessorsUsersService : IProfessorsUsersService
    {
        private readonly IProfessorsUsersRepository _professorsUsersRepository;

        public ProfessorsUsersService(IProfessorsUsersRepository professorsUsersRepository)
        {
            _professorsUsersRepository = professorsUsersRepository;
        }

        public async Task AddProfessorsUsersAsync(int professorId, int userId)
        {
            var professorUser = new ProfessorsUsers
            {
                ProfessorId = professorId,
                UserId = userId
            };

            await _professorsUsersRepository.AddAsync(professorUser);
        }

        public async Task<List<int>> GetUserIdsByProfessorIdAsync(int professorId)
        {
            return await _professorsUsersRepository.GetUserIdsByProfessorIdAsync(professorId);
        }

        public async Task RemoveProfessorsUsersAsync(int professorId, int userId)
        {
            await _professorsUsersRepository.RemoveAsync(professorId, userId);
        }

        public async Task<bool> ProfessorsUsersExistsAsync(int professorId, int userId)
        {
            return await _professorsUsersRepository.ExistsAsync(professorId, userId);
        }
    }
}
