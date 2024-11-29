using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GPESAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentController : ControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly ITeamAppService _teamAppService;
        private readonly IProjectAppService _projectAppService;

        public StudentController(IUserAppService userAppService, ITeamAppService teamAppService, IProjectAppService projectAppService)
        {
            _userAppService = userAppService;
            _teamAppService = teamAppService;
            _projectAppService = projectAppService;
        }

        [HttpGet("project-team-view")]
        public async Task<IActionResult> ProjectTeamView()
        {
            var studentNumber = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(studentNumber))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _projectAppService.StudentProjectTeamView(studentNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("create-team")]
        public async Task<IActionResult> CreateTeam(TeamCreator teamCreator)
        {
            var studentNumber = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(studentNumber))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _teamAppService.CreateTeamAsync(studentNumber, teamCreator);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
