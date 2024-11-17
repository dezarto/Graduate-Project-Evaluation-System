using GPESAPI.Application.Interfaces;
using GPESAPI.Application.Services;
using GPESAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;
using GraduateProjectEvaluationSystemAPI.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduateProjectEvaluationSystemAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManuelController : ControllerBase
    {
        private readonly IProfessorsUsersAppService _professorsUsersAppService;
        private readonly IUserAppService _userAppService;
        private readonly ITeamAppService _teamAppService;
        private readonly IProfessorAvailabilityAppService _professorAvailabilityAppService;
        private readonly ITeamPresentationAppService _teamPresentationAppService;

        public ManuelController(IProfessorsUsersAppService professorsUsersAppService, IUserAppService userAppService, ITeamAppService teamAppService, IProfessorAvailabilityAppService professorAvailabilityAppService, ITeamPresentationAppService teamPresentationAppService)
        {
            _professorsUsersAppService = professorsUsersAppService;
            _userAppService = userAppService;
            _teamAppService = teamAppService;
            _professorAvailabilityAppService = professorAvailabilityAppService;
            _teamPresentationAppService = teamPresentationAppService;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncUsersWithProfessors()
        {
            try
            {
                var usersWithProfessors = await _userAppService.GetUsersWithProfessorsAppAsync();

                foreach (var user in usersWithProfessors)
                {
                    bool exists = await _professorsUsersAppService.ProfessorsUsersExistsAppAsync(user.ProfessorId, user.UserId);
                    if (!exists)
                    {
                        await _professorsUsersAppService.AddProfessorsUsersAppAsync(user.ProfessorId, user.UserId);
                    }
                }

                return Ok(new { message = "Users synchronized successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while syncing users.", error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("teams/schedule")]
        public async Task<IActionResult> ScheduleTeamsPresentations()
        {
            try
            {
                var teams = await _teamAppService.GetAllTeamAppAsync(); // Tüm takımları al
                var availabilities = await _professorAvailabilityAppService.GetAllProfessorAvailabilityAppAsync(); // Tüm hocaların müsaitliklerini al
                var presentations = new List<TeamPresentation>();

                foreach (var team in teams)
                {
                    var advisorAvailability = availabilities
                        .Where(a => a.ProfessorId == team.AdvisorId)
                        .OrderBy(a => a.AvailableDate)
                        .ThenBy(a => a.StartTime)
                        .FirstOrDefault();

                    if (advisorAvailability == null)
                        return Conflict(new { message = $"Advisor for Team {team.TeamId} is not available." });

                    var startTime = advisorAvailability.StartTime;
                    var endTime = startTime.AddMinutes(30);

                    var otherProfessors = availabilities
                        .Where(a => a.ProfessorId != team.AdvisorId && a.AvailableDate == advisorAvailability.AvailableDate)
                        .Where(a => a.StartTime <= startTime && a.EndTime >= endTime)
                        .Select(a => a.ProfessorId)
                        .Distinct()
                        .Take(2)
                        .ToList();

                    if (otherProfessors.Count < 2)
                        return Conflict(new { message = $"Not enough professors available for Team {team.TeamId} on {advisorAvailability.AvailableDate}." });

                    var presentation = new TeamPresentation
                    {
                        TeamId = team.TeamId,
                        ProjectId = team.ProjectId,
                        AdvisorId = team.AdvisorId,
                        Professor1Id = otherProfessors[0],
                        Professor2Id = otherProfessors[1],
                        PresentationDate = advisorAvailability.AvailableDate,
                        StartTime = startTime,
                        EndTime = endTime
                    };

                    presentations.Add(presentation);

                    // Müsaitlik güncellemelerini uygula
                    await _professorAvailabilityAppService.UpdateAvailabilityAsync(team.AdvisorId, advisorAvailability.AvailableDate, startTime, endTime);
                    foreach (var professorId in otherProfessors)
                    {
                        await _professorAvailabilityAppService.UpdateAvailabilityAsync(professorId, advisorAvailability.AvailableDate, startTime, endTime);
                    }
                }
                
                await _teamPresentationAppService.SaveAllPresentationsAsync(presentations);
                return Ok(new { message = "All teams scheduled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while scheduling teams.", error = ex.Message });
            }
        }

    }
}
