using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.BiDi.Modules.Script;
using System.Security.Claims;

namespace GPESAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {
        private readonly IProfessorAppService _professorAppService;
        private readonly IEvaluationAppService _evaluationAppService;
        private readonly IProjectAppService _projectAppService;
        private readonly IProfessorAvailabilityAppService _professorAvailabilityAppService;

        public ProfessorController(IProfessorAppService professorAppService, IEvaluationAppService evaluationAppService, IProjectAppService projectAppService, IProfessorAvailabilityAppService professorAvailabilityAppService)
        {
            _professorAppService = professorAppService;
            _evaluationAppService = evaluationAppService;
            _projectAppService = projectAppService;
            _professorAvailabilityAppService = professorAvailabilityAppService;
        }

        [HttpGet("get-availability-by-professor-auth")]
        public async Task<ActionResult> GetProfessorAvailability()
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized("User email is not available.");
            }

            try
            {
                var availabilityResult = await _professorAvailabilityAppService.GetProfessorAvailabilityAppByEmailAsync(professorMail);
                
                return Ok(availabilityResult);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already exists"))
                {
                    return Conflict(new { message = ex.Message });
                }

                return StatusCode(500, new { message = "An error occurred while adding availability data.", error = ex.Message });
            }
        }

        [HttpPost("post-availability-by-professor")]
        public async Task<ActionResult> AddProfessorAvailability([FromBody] ProfessorAvailabilityDTO availabilities)
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized("User email is not available.");
            }

            try
            {
                await _professorAvailabilityAppService.AddProfessorAvailabilityBatchAsync(professorMail, availabilities);
                return Ok(new { message = "Availability data added successfully." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already exists"))
                {
                    return Conflict(new { message = ex.Message });
                }

                return StatusCode(500, new { message = "An error occurred while adding availability data.", error = ex.Message });
            }
        }

        [HttpDelete("delete-availability-by-id/{id}")]
        public async Task<ActionResult> AddProfessorAvailability(int id)
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized("User email is not available.");
            }

            try
            {
                await _professorAvailabilityAppService.DeleteProfessorAvailabilityAppAsync(id);
                
                return Ok(new { message = "Availability deleted successfully." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already exists"))
                {
                    return Conflict(new { message = ex.Message });
                }

                return StatusCode(500, new { message = "An error occurred while adding availability data.", error = ex.Message });
            }
        }

        [HttpGet("get-approval-teams-view")]
        public async Task<ActionResult> ProfessorApprovalTeamsView()
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized("User email is not available.");
            }

            try
            {
                var result = await _professorAppService.ProfessorApprovalTeamsView(professorMail);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("post-approval-teams")]
        public async Task<ActionResult> ProfessorApprovalTeams(int teamId, bool approval)
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized("User email is not available.");
            }

            try
            {
                var result = await _professorAppService.ProfessorApprovalTeams(teamId, approval);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get-my-profile")]
        public async Task<ActionResult> MyProfile()
        {
            var mailAdress = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(mailAdress))
            {
                return Unauthorized("User email is not available.");
            }

            var professor = await _professorAppService.GetByProfessorAppEmailAsync(mailAdress);

            return Ok(professor);
        }

        [HttpGet("get-project-team-view")]
        public async Task<IActionResult> ProjectTeamView()
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _projectAppService.ProfessorProjectTeamView(professorMail);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("submit-evaluation")]
        public async Task<IActionResult> SubmitEvaluation([FromBody] EvaluateReasult evaluateResult)
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized();
            }

            if (evaluateResult == null)
                return BadRequest("Evaluation result is null.");

            var result = await _evaluationAppService.SubmitEvaluationSave(evaluateResult, professorMail);
            
            if (result)
            {
                return Ok("Evaluation submitted successfully.");
            }

            return BadRequest("Evaluation submitted failed.");
        }

        [HttpGet("get-evaluation/{evaluationId}")]
        public async Task<IActionResult> GetEvaluation(int evaluationId)
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized();
            }

            if (evaluationId == 0)
                return BadRequest("Evaluation result is null.");

            var result = await _evaluationAppService.GetEvaluationResult(evaluationId);

            return Ok(result);
        }

        [HttpGet("get-project-team-result/{teamId}")]
        public async Task<IActionResult> ProjectTeamResult(int teamId)
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _projectAppService.ProfessorProjectTeamResult(professorMail, teamId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get-evaluations-criteria-and-check-list-datas")]
        public async Task<IActionResult> EvaluationsCriteriaAndCheckListDatas()
        {
            var professorMail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(professorMail))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _evaluationAppService.GetAllCriterias();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
