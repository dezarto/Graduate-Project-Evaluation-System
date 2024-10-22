using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Domain.Interfaces
{
    public interface IProfessorAvailabilityService
    {
        Task<IEnumerable<ProfessorAvailability>> GetAllProfessorAvailabilitiesAsync();
        Task<ProfessorAvailability> GetProfessorAvailabilityByIdAsync(int id);
        Task AddProfessorAvailabilityAsync(ProfessorAvailability professorAvailability);
        Task UpdateProfessorAvailabilityAsync(ProfessorAvailability professorAvailability);
        Task DeleteProfessorAvailabilityAsync(int id);
        Task<bool> CheckExistingAvailabilityAsync(int professorId, DateTime availableDate, TimeSpan startTime, TimeSpan endTime);
    }
}
