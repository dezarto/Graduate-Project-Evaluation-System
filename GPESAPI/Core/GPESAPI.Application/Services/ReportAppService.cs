using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace GPESAPI.Application.Services
{
    public class ReportAppService : IReportAppService
    {
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public ReportAppService(IReportService reportService, ITeamMemberService teamMemberService, IUserService userService, ITeamService teamService , IMapper mapper)
        {
            _reportService = reportService;
            _userService = userService;
            _teamMemberService = teamMemberService;
            _teamService = teamService;
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

        public async Task<bool> UploadReport(string filePath, string studentNumber)
        {
            try
            {
                var user = await _userService.GetByStudentNumberAsync(studentNumber);
                if (user == null) return false;

                var teamMember = await _teamMemberService.GetByUserIdAsync(user.UserId);
                if (teamMember == null) return false;

                var team = await _teamService.GetTeamByIdAsync(teamMember.UserId);

                var newReport = new Report
                {
                    ReportDate = DateTime.Now,
                    TeamId = teamMember.TeamId,
                    FilePath = filePath,
                };

                await _reportService.UpdateReportAsync(newReport);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
