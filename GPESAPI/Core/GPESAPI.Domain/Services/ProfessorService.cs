using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Domain.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _professorRepository;
        public ProfessorService(IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        public async Task<Professor> AddProfessorAsync(Professor professor)
        {
            return await _professorRepository.AddAsync(professor);
        }

        public async Task DeleteProfessorAsync(int id)
        {
            await _professorRepository.DeleteAsync(id);
        }

        public Task<List<Professor>> GetAllProfessorAsync()
        {
            return _professorRepository.GetAllAsync();
        }

        public async Task<Professor> GetByProfessorIdAsync(int id)
        {
            return await _professorRepository.GetByIdAsync(id);
        }

        public async Task UpdateProfessorAsync(Professor professor)
        {
            await _professorRepository.UpdateAsync(professor);
        }
    }
}
