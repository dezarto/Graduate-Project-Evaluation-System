using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Application.Services
{
    public class ProfessorAvailabilityAppService : IProfessorAvailabilityAppService
    {
        private readonly IProfessorAvailabilityService _professorAvailabilityService;
        private readonly IProfessorService _professorService;
        private readonly IMapper _mapper;

        public ProfessorAvailabilityAppService(IProfessorAvailabilityService professorAvailabilityService, IProfessorService professorService , IMapper mapper)
        {
            _professorAvailabilityService = professorAvailabilityService;
            _professorService = professorService;
            _mapper = mapper;
        }

        public async Task AddProfessorAvailabilityBatchAsync(string professorMail, ProfessorAvailabilityDTO availabilities)
        {
            var professor = await _professorService.GetByProfessorEmailAsync(professorMail);

            availabilities.ProfessorId = professor.ProfessorId;

            var existingAvailability = await _professorAvailabilityService.CheckExistingAvailabilityAsync(
                professor.ProfessorId, availabilities.AvailableDate, availabilities.StartTime, availabilities.EndTime);

            if (existingAvailability)
            {
                throw new Exception("An availability record already exists for the specified date and time range.");
            }

            TimeSpan startTime = availabilities.StartTime;
            TimeSpan endTime = availabilities.EndTime;

            while (startTime < endTime)
            {
                TimeSpan potentialEndTime = startTime + TimeSpan.FromMinutes(30);

                if (potentialEndTime > endTime)
                {
                    break;
                }

                var availability = new ProfessorAvailability
                {
                    ProfessorId = professor.ProfessorId,
                    AvailableDate = availabilities.AvailableDate,
                    StartTime = startTime,
                    EndTime = potentialEndTime
                };

                await _professorAvailabilityService.AddProfessorAvailabilityAsync(availability);

                startTime = potentialEndTime;
            }
        }

        public async Task<IEnumerable<ProfessorAvailabilityDTO>> GetAllProfessorAvailabilityAppAsync()
        {
            var availabilities = await _professorAvailabilityService.GetAllProfessorAvailabilitiesAsync();
            return _mapper.Map<IEnumerable<ProfessorAvailabilityDTO>>(availabilities);
        }

        public async Task<List<ProfessorAvailabilityDTO>> GetProfessorAvailabilityAppByEmailAsync(string professorMail)
        {
            var professor = await _professorService.GetByProfessorEmailAsync(professorMail);
            var availability = await _professorAvailabilityService.GetProfessorAvailabilityByIdAsync(professor.ProfessorId);
            return _mapper.Map<List<ProfessorAvailabilityDTO>>(availability);
        }

        public async Task AddProfessorAvailabilityAppAsync(ProfessorAvailabilityDTO availabilityDto)
        {
            var availability = _mapper.Map<ProfessorAvailability>(availabilityDto);
            await _professorAvailabilityService.AddProfessorAvailabilityAsync(availability);
        }

        public async Task UpdateProfessorAvailabilityAppAsync(ProfessorAvailabilityDTO availabilityDto)
        {
            var availability = _mapper.Map<ProfessorAvailability>(availabilityDto);
            await _professorAvailabilityService.UpdateProfessorAvailabilityAsync(availability);
        }

        public async Task DeleteProfessorAvailabilityAppAsync(int id)
        {
            await _professorAvailabilityService.DeleteProfessorAvailabilityAsync(id);
        }

        public async Task<bool> CheckExistingAvailabilityAppAsync(int professorId, DateTime availableDate, TimeSpan startTime, TimeSpan endTime)
        {
            return await _professorAvailabilityService.CheckExistingAvailabilityAsync(professorId, availableDate, startTime, endTime);
        }
    }
}
