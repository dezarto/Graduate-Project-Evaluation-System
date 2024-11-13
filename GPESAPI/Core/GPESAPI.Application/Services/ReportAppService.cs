using AutoMapper;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Application.Services
{
    public class ReportAppService : IReportAppService
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public ReportAppService(IReportService reportService, IMapper mapper)
        {
            _reportService = reportService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReportDTO>> GetAllReportAppAsync()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return _mapper.Map<IEnumerable<ReportDTO>>(reports);
        }

        public async Task<ReportDTO> GetReportAppByIdAsync(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            return _mapper.Map<ReportDTO>(report);
        }

        public async Task AddReportAppAsync(ReportDTO reportDto)
        {
            var report = _mapper.Map<Report>(reportDto);
            await _reportService.AddReportAsync(report);
        }

        public async Task UpdateReportAppAsync(ReportDTO reportDto)
        {
            var report = _mapper.Map<Report>(reportDto);
            await _reportService.UpdateReportAsync(report);
        }

        public async Task DeleteReportAppAsync(int id)
        {
            await _reportService.DeleteReportAsync(id);
        }
    }
}
