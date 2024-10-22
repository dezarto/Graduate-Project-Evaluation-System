using GraduateProjectEvaluationSystemAPI.Application.DTOs;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface IPanelAppService
    {
        Task<IEnumerable<PanelDTO>> GetAllPanelAppAsync();
        Task<PanelDTO> GetPanelAppByIdAsync(int id);
        Task AddPanelAppAsync(PanelDTO panelDto);
        Task UpdatePanelAppAsync(PanelDTO panelDto);
        Task DeletePanelAppAsync(int id);
    }
}
