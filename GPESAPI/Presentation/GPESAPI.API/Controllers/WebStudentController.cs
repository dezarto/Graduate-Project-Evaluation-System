using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GPESAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class WebStudentController : ControllerBase
    {
        private readonly IProfessorsUsersAppService _professorsUsersAppService;
        private readonly IUserAppService _userAppService;
        private readonly IProfessorAppService _professorAppService;
        private readonly ITeamAppService _teamAppService;
        private readonly ITeamMemberAppService _teamMemberAppService;
        private readonly IProfessorAvailabilityAppService _professorAvailabilityAppService;
        private readonly ITeamPresentationAppService _teamPresentationAppService;
        private readonly IProjectAppService _projectAppService;

        public WebStudentController(IProfessorsUsersAppService professorsUsersAppService, IUserAppService userAppService, ITeamAppService teamAppService, IProfessorAvailabilityAppService professorAvailabilityAppService, ITeamPresentationAppService teamPresentationAppService, IProfessorAppService professorAppService, IProjectAppService projectAppService, ITeamMemberAppService teamMemberAppService)
        {
            _professorsUsersAppService = professorsUsersAppService;
            _userAppService = userAppService;
            _teamAppService = teamAppService;
            _professorAvailabilityAppService = professorAvailabilityAppService;
            _teamPresentationAppService = teamPresentationAppService;
            _professorAppService = professorAppService;
            _projectAppService = projectAppService;
            _teamMemberAppService = teamMemberAppService;
        }

        [HttpGet("projectTeamView")]
        public async Task<IActionResult> ProjectTeamView()
        {
            var studentNumber = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(studentNumber))
            {
                return Unauthorized();
            }

            var student = await _userAppService.GetByStudentNumberAsync(studentNumber);
            var teamId = await _teamMemberAppService.GetTeamMemberByUserIdAsync(student.UserId);
            var team = await _teamAppService.GetTeamAppByIdAsync(teamId.TeamId);
            var project = await _projectAppService.GetProjectAppByIdAsync(team.ProjectId);
            var teamMembers = await _teamMemberAppService.GetTeamMemberByTeamIdAsync(team.TeamId);

            try
            {
                var newStudentProjectTeamsWeb = new StudentProjectTeamsWeb
                {
                    TeamId = team.TeamId,
                    ProjectId = team.ProjectId,
                    AdvisorId = team.AdvisorId,
                    isActive = team.isActive,
                    TeamName = team.TeamName,
                    ProjectName = project.ProjectName,
                    Description = project.Description,
                    TeamMembers = new List<MemberList>()
                };

                if (teamMembers != null && teamMembers.Count > 0)
                {
                    foreach (var teamMember in teamMembers)
                    {
                        var studentInfos = await _userAppService.GetByUserAppIdAsync(teamMember.UserId);

                        var studentDatas = new MemberList
                        {
                            StudentId = teamMember.UserId,
                            StudentFullName = studentInfos.FullName,
                            StudentNumber = studentInfos.StudentNumber,
                        };

                        newStudentProjectTeamsWeb.TeamMembers.Add(studentDatas);
                    }
                }

                return Ok(newStudentProjectTeamsWeb);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching presentations.", error = ex.Message });
            }
        }
    }
}
