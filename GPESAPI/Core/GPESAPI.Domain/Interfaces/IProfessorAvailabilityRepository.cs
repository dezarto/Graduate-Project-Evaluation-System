namespace GPESAPI.Domain.Interfaces
{
    public interface IProfessorAvailabilityRepository
    {
        Task<bool> CheckExistingAvailabilityAsync(int professorId, DateTime availableDate, TimeSpan startTime, TimeSpan endTime);
    }
}
