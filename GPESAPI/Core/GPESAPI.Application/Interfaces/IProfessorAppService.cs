using GPESAPI.Application.DTOs;

namespace GPESAPI.Application.Interfaces
{
    public interface IProfessorAppService
    {
        Task<ProfessorDTO> AddProfessorAppAsync(ProfessorDTO professorDto);
        Task<List<ProfessorDTO>> GetAllProfessorAppAsync();
        Task<ProfessorDTO> GetByProfessorAppIdAsync(int id);
        Task<ProfessorDTO> GetByProfessorAppEmailAsync(string email);

        Task UpdateProfessorAppAsync(ProfessorDTO professorDto);
        Task DeleteProfessorAppAsync(int id);
        Task<string> ProfessorApprovalTeams(int teamId, bool approval);
        Task<List<TeamDTO>> ProfessorApprovalTeamsView(string professorMail);
    }
}
