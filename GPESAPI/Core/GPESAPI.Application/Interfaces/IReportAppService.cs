using GPESAPI.Application.DTOs;

namespace GPESAPI.Application.Interfaces
{
    public interface IReportAppService
    {
        Task<IEnumerable<ReportDTO>> GetAllReportAppAsync();
        Task<ReportDTO> GetReportAppByIdAsync(int id);
        Task AddReportAppAsync(ReportDTO reportDto);
        Task UpdateReportAppAsync(ReportDTO reportDto);
        Task DeleteReportAppAsync(int id);
    }
}
