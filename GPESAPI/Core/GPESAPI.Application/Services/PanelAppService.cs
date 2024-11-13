using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class PanelAppService : IPanelAppService
    {
        private readonly IPanelService _panelService;
        private readonly IMapper _mapper;

        public PanelAppService(IPanelService panelService, IMapper mapper)
        {
            _panelService = panelService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PanelDTO>> GetAllPanelAppAsync()
        {
            var panels = await _panelService.GetAllPanelsAsync();
            return _mapper.Map<IEnumerable<PanelDTO>>(panels);
        }

        public async Task<PanelDTO> GetPanelAppByIdAsync(int id)
        {
            var panel = await _panelService.GetPanelByIdAsync(id);
            return _mapper.Map<PanelDTO>(panel);
        }

        public async Task AddPanelAppAsync(PanelDTO panelDto)
        {
            var panel = _mapper.Map<Panel>(panelDto);
            await _panelService.AddPanelAsync(panel);
        }

        public async Task UpdatePanelAppAsync(PanelDTO panelDto)
        {
            var panel = _mapper.Map<Panel>(panelDto);
            await _panelService.UpdatePanelAsync(panel);
        }

        public async Task DeletePanelAppAsync(int id)
        {
            await _panelService.DeletePanelAsync(id);
        }
    }
}
