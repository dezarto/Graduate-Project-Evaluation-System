using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GraduateProjectEvaluationSystemAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : Controller
    {
        private readonly IProfessorAppService _professorAppService;
        private readonly ITeamAppService _teamAppService;
        private readonly IProjectAppService _projectAppService;
        private readonly ITeamMemberAppService _teamMemberAppService;
        private readonly IProfessorAvailabilityAppService _professorAvailabilityAppService;

        public ProfessorController(IProfessorAppService professorAppService, IProfessorAvailabilityAppService professorAvailabilityAppService, ITeamAppService teamAppService, IProjectAppService projectAppService, ITeamMemberAppService teamMemberAppService)
        {
            _professorAppService = professorAppService;
            _professorAvailabilityAppService = professorAvailabilityAppService;
            _teamAppService = teamAppService;
            _projectAppService = projectAppService;
            _teamMemberAppService = teamMemberAppService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProfessorDTO>> CreateProfessor([FromBody] ProfessorDTO professorDto)
        {
            await _professorAppService.AddProfessorAppAsync(professorDto);
            return Ok(professorDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAllProfessors")]
        public async Task<ActionResult<List<ProfessorDTO>>> GetAllProfessors()
        {
            return await _professorAppService.GetAllProfessorAppAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfessorDTO>> GetProfessorById(int id)
        {
            var professor = await _professorAppService.GetByProfessorAppIdAsync(id);
            if (professor == null)
            {
                return NotFound();
            }
            return professor;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfessor(int id, [FromBody] ProfessorDTO professorDto)
        {
            if (id != professorDto.ProfessorId)
            {
                return BadRequest();
            }

            await _professorAppService.UpdateProfessorAppAsync(professorDto);
            return Ok(professorDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfessor(int id)
        {
            await _professorAppService.DeleteProfessorAppAsync(id);
            return Ok("Successful");
        }

        [Authorize(Roles = "Student,Professor")]
        [HttpPost("{professorId}/availability")]
        public async Task<ActionResult> AddProfessorAvailability(int professorId, [FromBody] List<ProfessorAvailabilityDTO> availabilities)
        {
            try
            {
                foreach (var availabilityDto in availabilities)
                {
                    // ProfessorId zaten mevcut, her availability kaydında set ediyoruz
                    availabilityDto.ProfessorId = professorId;

                    // 1. Adım: Veritabanında belirtilen tarih ve saat aralığında bir kaydın olup olmadığını kontrol ediyoruz
                    var existingAvailability = await _professorAvailabilityAppService.CheckExistingAvailabilityAppAsync(
                        professorId, availabilityDto.AvailableDate, availabilityDto.StartTime, availabilityDto.EndTime);

                    if (existingAvailability)
                    {
                        // Eğer aynı zaman diliminde zaten bir kayıt varsa, hata döndürüyoruz
                        return Conflict(new { message = "An availability record already exists for the specified date and time range." });
                    }

                    // 2. Adım: Eğer kayıt yoksa, Availability kaydediliyor
                    await _professorAvailabilityAppService.AddProfessorAvailabilityAppAsync(availabilityDto);
                }

                return Ok(new { message = "Availability data added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding availability data.", error = ex.Message });
            }
        }

        [Authorize(Roles = "Professor")]
        [HttpGet("approval-teams-view")]
        public async Task<ActionResult> ProfessorApprovalTeamsView()
        {
            var mailAdress = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(mailAdress))
            {
                return Unauthorized("User email is not available.");
            }

            var finProfessor = await _professorAppService.GetByProfessorAppEmailAsync(mailAdress);

            if (finProfessor == null)
            {
                return NotFound("Resource not found.");
            }

            var teamInfos = await _teamAppService.GetByAdvisorIdTeamAppAsync(finProfessor.ProfessorId);

            if (teamInfos == null || !teamInfos.Any())
            {
                return NotFound("No teams found.");
            }

            var enrichedTeamInfos = teamInfos.Select(team => new TeamDTO
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                ProjectId = team.ProjectId,
                AdvisorId = team.AdvisorId,
                isActive = team.isActive
            }).ToList();

            return Ok(enrichedTeamInfos);
        }

        [Authorize(Roles = "Professor")]
        [HttpPost("approval-teams")]
        public async Task<ActionResult> ProfessorApprovalTeams(int teamId, bool approval)
        {
            var mailAdress = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(mailAdress))
            {
                return Unauthorized("User email is not available.");
            }

            var teamInfo = await _teamAppService.GetTeamAppByIdAsync(teamId);

            if (teamInfo == null)
            {
                return NotFound("No teams found.");
            }

            if (approval)
            {
                teamInfo.isActive = true;
                await _teamAppService.UpdateTeamAppAsync(teamInfo);
            }
            else
            {
                await _teamMemberAppService.DeleteTeamMemberAppAsync(teamInfo.TeamId);
                await _teamAppService.DeleteTeamAppAsync(teamId);
                await _projectAppService.DeleteProjectAppAsync(teamInfo.ProjectId);
            }

            return Ok();
        }

        [Authorize(Roles = "Professor")]
        [HttpGet("myProfile")]
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
    }
}
