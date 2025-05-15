using GPESAPI.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace GPESAPI.Application.Interfaces
{
    public interface IReportAppService
    {
        Task<IEnumerable<ReportDTO>> GetAllReportAppAsync();
        Task<ReportDTO> GetReportAppByIdAsync(int id);
        Task AddReportAppAsync(ReportDTO reportDto);
        Task UpdateReportAppAsync(ReportDTO reportDto);
        Task DeleteReportAppAsync(int id);
        Task<bool> UploadReport(string filePath, string studentNumber);
    }
}
