using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Domain.Services
{
    public class ProfessorAvailabilityService : IProfessorAvailabilityService
    {
        private readonly IGenericRepository<ProfessorAvailability> _professorAvailabilityRepository;
        private readonly IProfessorAvailabilityRepository _professorAvailabilityRepositoryMain;

        public ProfessorAvailabilityService(IGenericRepository<ProfessorAvailability> professorAvailabilityRepository, IProfessorAvailabilityRepository professorAvailabilityRepositoryMain)
        {
            _professorAvailabilityRepository = professorAvailabilityRepository;
            _professorAvailabilityRepositoryMain = professorAvailabilityRepositoryMain;
        }

        public async Task<IEnumerable<ProfessorAvailability>> GetAllProfessorAvailabilitiesAsync()
        {
            return await _professorAvailabilityRepository.GetAllAsync();
        }

        public async Task<ProfessorAvailability> GetProfessorAvailabilityByIdAsync(int id)
        {
            return await _professorAvailabilityRepository.GetByIdAsync(id);
        }

        public async Task AddProfessorAvailabilityAsync(ProfessorAvailability professorAvailability)
        {
            await _professorAvailabilityRepository.AddAsync(professorAvailability);
        }

        public async Task UpdateProfessorAvailabilityAsync(ProfessorAvailability professorAvailability)
        {
            await _professorAvailabilityRepository.UpdateAsync(professorAvailability);
        }

        public async Task DeleteProfessorAvailabilityAsync(int id)
        {
            await _professorAvailabilityRepository.DeleteAsync(id);
        }

        public async Task<bool> CheckExistingAvailabilityAsync(int professorId, DateTime availableDate, TimeSpan startTime, TimeSpan endTime)
        {
            return await _professorAvailabilityRepositoryMain.CheckExistingAvailabilityAsync(professorId, availableDate, startTime, endTime);
        }
    }

}
