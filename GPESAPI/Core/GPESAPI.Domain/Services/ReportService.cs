using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Domain.Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<Report> _reportRepository;

        public ReportService(IGenericRepository<Report> reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _reportRepository.GetAllAsync();
        }

        public async Task<Report> GetReportByIdAsync(int id)
        {
            return await _reportRepository.GetByIdAsync(id);
        }

        public async Task AddReportAsync(Report report)
        {
            await _reportRepository.AddAsync(report);
        }

        public async Task UpdateReportAsync(Report report)
        {
            await _reportRepository.UpdateAsync(report);
        }

        public async Task DeleteReportAsync(int id)
        {
             await _reportRepository.DeleteAsync(id);
        }
    }

}
