using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GPESAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Professor")]
    public class MobileController : ControllerBase
    {
        private readonly IProfessorsUsersAppService _professorsUsersAppService;
        private readonly IUserAppService _userAppService;
        private readonly IProfessorAppService _professorAppService;
        private readonly ITeamAppService _teamAppService;
        private readonly IProfessorAvailabilityAppService _professorAvailabilityAppService;
        private readonly ITeamPresentationAppService _teamPresentationAppService;
        private readonly IProjectAppService _projectAppService;

        public MobileController(IProfessorsUsersAppService professorsUsersAppService, IUserAppService userAppService, ITeamAppService teamAppService, IProfessorAvailabilityAppService professorAvailabilityAppService, ITeamPresentationAppService teamPresentationAppService, IProfessorAppService professorAppService, IProjectAppService projectAppService)
        {
            _professorsUsersAppService = professorsUsersAppService;
            _userAppService = userAppService;
            _teamAppService = teamAppService;
            _professorAvailabilityAppService = professorAvailabilityAppService;
            _teamPresentationAppService = teamPresentationAppService;
            _professorAppService = professorAppService;
            _projectAppService = projectAppService;
        }

        [HttpGet("projectTeamView")]
        public async Task<IActionResult> ProjectTeamView()
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized();
            }

            var professor = await _professorAppService.GetByProfessorAppEmailAsync(professorMail);

            try
            {
                var presentations = await _teamPresentationAppService.GetTeamPresentationByIdAsync(professor.ProfessorId);

                if (presentations == null || presentations.Count == 0)
                {
                    return NotFound(new { message = "No presentations found for the given professor ID." });
                }

                var projectTeamsMobileList = new List<ProjectTeamsMobile>();

                foreach (var presentation in presentations) 
                {
                    var project = await _projectAppService.GetProjectAppByIdAsync(presentation.ProjectId);
                    var team = await _teamAppService.GetTeamAppByIdAsync(presentation.TeamId);
                    var professorInfos = await _professorAppService.GetByProfessorAppIdAsync(professor.ProfessorId);

                    var newProjectTeamsMobile = new ProjectTeamsMobile
                    {
                        TeamPresentationId = presentation.TeamPresentationId,
                        TeamId = presentation.TeamId,
                        ProjectName = project.ProjectName,
                        Description = project.Description,
                        TeamName = team.TeamName,
                        isEvaluated = presentation.isEvaluated,
                        EvaluatingTeacherFullName = professorInfos.FullName,
                        EvaluatingTeacherMail = professorInfos.mailAddress,

                    };

                    projectTeamsMobileList.Add(newProjectTeamsMobile);
                }

                return Ok(projectTeamsMobileList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching presentations.", error = ex.Message });
            }
        }
    }
}
