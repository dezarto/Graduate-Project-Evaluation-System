using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IProfessorAvailabilityService
    {
        Task<IEnumerable<ProfessorAvailability>> GetAllProfessorAvailabilitiesAsync();
        Task<List<ProfessorAvailability>> GetProfessorAvailabilityByIdAsync(int id);
        Task AddProfessorAvailabilityAsync(ProfessorAvailability professorAvailability);
        Task UpdateProfessorAvailabilityAsync(ProfessorAvailability professorAvailability);
        Task DeleteProfessorAvailabilityAsync(int id);
        Task<bool> CheckExistingAvailabilityAsync(int professorId, DateTime availableDate, TimeSpan startTime, TimeSpan endTime);
    }
}
