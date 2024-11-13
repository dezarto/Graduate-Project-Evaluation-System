using GPESAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Application.Services;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GPESAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly IUserAppService _userAppService;
        private readonly ITeamAppService _teamAppService;
        private readonly IProjectAppService _projectAppService;
        private readonly ITeamMemberAppService _teamMemberAppService;

        public TeamController(IUserAppService userAppService, ITeamAppService teamAppService, IProjectAppService projectAppService, ITeamMemberAppService teamMemberAppService)
        {
            _userAppService = userAppService;
            _teamAppService = teamAppService;
            _projectAppService = projectAppService;
            _teamMemberAppService = teamMemberAppService;
        }

        [HttpPost("createTeam")]
        public async Task<IActionResult> CreateTeam(TeamCreator teamCreator)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            var student = await _userAppService.GetByStudentNumberAsync(username);
            
            var newProject = new ProjectDTO
            {
                Description = teamCreator.Description,
                ProjectName = teamCreator.ProjectName,
            };

            await _projectAppService.AddProjectAppAsync(newProject);

            var project = await _projectAppService.GetProjectAppByIdAsync(newProject.ProjectId);

            if (project == null)
            {
                return NotFound("Project creation failed.");
            }

            var newTeam = new TeamDTO
            {
                TeamName = teamCreator.TeamName,
                AdvisorId = student.ProfessorId,
                ProjectId = project.ProjectId,
            };

            await _teamAppService.AddTeamAppAsync(newTeam);

            foreach (var studentUsername in teamCreator.StudentList)
            {
                var studentL = await _userAppService.GetByStudentNumberAsync(studentUsername.StudenNumber);

                if (studentL == null)
                {
                    Console.WriteLine($"Student not found: {studentUsername.StudenNumber}");
                    continue;
                }

                var newTeamMember = new TeamMemberDTO
                {
                    TeamId = newTeam.TeamId,
                    UserId = studentL.UserId,
                };

                await _teamMemberAppService.AddTeamMemberAppAsync(newTeamMember);
            }

            return Ok(new { TeamId = newTeam.TeamId, ProjectId = project.ProjectId, Message = "Team created successfully" }); ;
        }
    }
}
