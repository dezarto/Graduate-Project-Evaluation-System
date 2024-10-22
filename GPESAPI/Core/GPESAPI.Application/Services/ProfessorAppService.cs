using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class ProfessorAppService : IProfessorAppService
    {
        private readonly IProfessorService _professorService;
        private readonly IMapper _mapper;

        public ProfessorAppService(IProfessorService professorService, IMapper mapper)
        {
            _professorService = professorService;
            _mapper = mapper;
        }

        public async Task<List<ProfessorDTO>> GetAllProfessorAppAsync()
        {
            var professors = await _professorService.GetAllProfessorAsync();
            return _mapper.Map<List<ProfessorDTO>>(professors); // Task<List<ProfessorDTO>> döndürülmeli
        }

        public async Task<ProfessorDTO> GetByProfessorAppIdAsync(int id)
        {
            var professor = await _professorService.GetByProfessorIdAsync(id);
            return _mapper.Map<ProfessorDTO>(professor); // Metod adı interface ile uyumlu olmalı
        }

        public async Task<ProfessorDTO> AddProfessorAppAsync(ProfessorDTO professorDto)
        {
            var professor = _mapper.Map<Professor>(professorDto);
            var addedProfessor = await _professorService.AddProfessorAsync(professor);
            return _mapper.Map<ProfessorDTO>(addedProfessor);
        }

        public async Task UpdateProfessorAppAsync(ProfessorDTO professorDto)
        {
            var professor = _mapper.Map<Professor>(professorDto);
            await _professorService.UpdateProfessorAsync(professor);
        }

        public async Task DeleteProfessorAppAsync(int id)
        {
            await _professorService.DeleteProfessorAsync(id);
        }
    }
}
