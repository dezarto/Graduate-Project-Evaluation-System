using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class ProfessorAvailabilityAppService : IProfessorAvailabilityAppService
    {
        private readonly IProfessorAvailabilityService _professorAvailabilityService;
        private readonly IMapper _mapper;

        public ProfessorAvailabilityAppService(IProfessorAvailabilityService professorAvailabilityService, IMapper mapper)
        {
            _professorAvailabilityService = professorAvailabilityService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProfessorAvailabilityDTO>> GetAllProfessorAvailabilityAppAsync()
        {
            var availabilities = await _professorAvailabilityService.GetAllProfessorAvailabilitiesAsync();
            return _mapper.Map<IEnumerable<ProfessorAvailabilityDTO>>(availabilities);
        }

        public async Task<ProfessorAvailabilityDTO> GetProfessorAvailabilityAppByIdAsync(int id)
        {
            var availability = await _professorAvailabilityService.GetProfessorAvailabilityByIdAsync(id);
            return _mapper.Map<ProfessorAvailabilityDTO>(availability);
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
