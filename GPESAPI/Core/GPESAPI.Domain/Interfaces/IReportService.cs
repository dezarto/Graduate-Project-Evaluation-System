using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(int id);
        Task AddReportAsync(Report report);
        Task UpdateReportAsync(Report report);
        Task DeleteReportAsync(int id);
        Task<Report> GetReportByTeamId(int id);
    }
}
