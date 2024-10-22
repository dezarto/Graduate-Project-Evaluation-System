using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Domain.Interfaces
{
    public interface IPanelService
    {
        Task<IEnumerable<Panel>> GetAllPanelsAsync();
        Task<Panel> GetPanelByIdAsync(int id);
        Task AddPanelAsync(Panel panel);
        Task UpdatePanelAsync(Panel panel);
        Task DeletePanelAsync(int id);
    }
}
