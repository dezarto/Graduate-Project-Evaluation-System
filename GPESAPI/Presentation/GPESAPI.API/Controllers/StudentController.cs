using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
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
        private readonly IReportAppService _reportAppService;
        private readonly ITeamAppService _teamAppService;
        private readonly IProjectAppService _projectAppService;

        public StudentController(IUserAppService userAppService, IReportAppService reportAppService, ITeamAppService teamAppService, IProjectAppService projectAppService)
        {
            _userAppService = userAppService;
            _teamAppService = teamAppService;
            _projectAppService = projectAppService;
            _reportAppService = reportAppService;
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

        [HttpPost("project-upload")]
        public async Task<IActionResult> UploadProject([FromForm] IFormFile file)
        {
            var studentNumber = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(studentNumber))
            {
                return Unauthorized();
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("File is missing or empty.");
            }

            try
            {
                var result = await _reportAppService.UploadReport(file, studentNumber);

                if (result)
                {
                    return Ok(new { Message = "File uploaded successfully." });
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while uploading the file.", Error = ex.Message });
            }
        }
    }
}
