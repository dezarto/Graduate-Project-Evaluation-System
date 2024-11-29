using GPESAPI.Application.DTOs;

namespace GPESAPI.Application.Interfaces
{
    public interface IProfessorAvailabilityAppService
    {
        Task<IEnumerable<ProfessorAvailabilityDTO>> GetAllProfessorAvailabilityAppAsync();
        Task<ProfessorAvailabilityDTO> GetProfessorAvailabilityAppByIdAsync(int id);
        Task AddProfessorAvailabilityAppAsync(ProfessorAvailabilityDTO professorAvailabilityDto);
        Task UpdateProfessorAvailabilityAppAsync(ProfessorAvailabilityDTO professorAvailabilityDto);
        Task DeleteProfessorAvailabilityAppAsync(int id);
        Task<bool> CheckExistingAvailabilityAppAsync(int professorId, DateTime availableDate, TimeSpan startTime, TimeSpan endTime);
        Task AddProfessorAvailabilityBatchAsync(int professorId, List<ProfessorAvailabilityDTO> availabilities);
    }
}
