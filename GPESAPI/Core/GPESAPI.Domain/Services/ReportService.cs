using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Domain.Services
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

            report.ReportId = report.ReportId;
        }

        public async Task DeleteReportAsync(int id)
        {
             await _reportRepository.DeleteAsync(id);
        }

        public async Task<Report> GetReportByTeamId(int id)
        {
            var detail = await _reportRepository.GetByFieldAsync("TeamId", id);
            if (detail == null)
                return null;

            return detail.FirstOrDefault();
        }
    }

}
