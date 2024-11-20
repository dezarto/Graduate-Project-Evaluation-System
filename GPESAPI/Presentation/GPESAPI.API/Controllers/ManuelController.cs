using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
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

        //[Authorize(Roles = "Admin")]
        [HttpPost("teams/schedule")]
        public async Task<IActionResult> ScheduleTeamsPresentations()
        {
            try
            {
                var teams = await _teamAppService.GetAllTeamAppAsync(); 
                var availabilities = await _professorAvailabilityAppService.GetAllProfessorAvailabilityAppAsync(); 
                var presentations = new List<TeamPresentationDTO>();
                var unassignedTeams = new List<TeamDTO>(); 

                var availabilitiesList = availabilities.ToList();

                foreach (var team in teams)
                {
                    var advisorAvailability = availabilitiesList
                        .Where(a => a.ProfessorId == team.AdvisorId)
                        .OrderBy(a => a.AvailableDate)
                        .ThenBy(a => a.StartTime)
                        .FirstOrDefault();

                    if (advisorAvailability == null)
                    {
                        unassignedTeams.Add(team);
                        continue;
                    }

                    var startTime = advisorAvailability.StartTime;
                    var endTime = startTime + TimeSpan.FromMinutes(30);

                    if (advisorAvailability.EndTime < endTime)
                    {
                        unassignedTeams.Add(team); 
                        continue;
                    }

                    List<int> otherProfessors = new List<int>();
                    bool foundSuitableProfessors = false;

                    
                    while (!foundSuitableProfessors)
                    {
                        
                        otherProfessors = availabilitiesList
                            .Where(a => a.ProfessorId != team.AdvisorId && a.AvailableDate == advisorAvailability.AvailableDate)
                            .Where(a => a.StartTime <= startTime && a.EndTime >= endTime)
                            .Select(a => a.ProfessorId)
                            .Distinct()
                            .Take(2)
                            .ToList();

                        if (otherProfessors.Count >= 2)
                        {
                            foundSuitableProfessors = true; 
                        }
                        else
                        {
                            var availableSlotsForSameDay = availabilitiesList
                                .Where(a => a.ProfessorId != team.AdvisorId && a.AvailableDate == advisorAvailability.AvailableDate)
                                .Where(a => a.StartTime > startTime && a.StartTime + TimeSpan.FromMinutes(30) <= a.EndTime)
                                .Select(a => new { a.ProfessorId, a.StartTime, a.EndTime })
                                .ToList();

                            foreach (var slot in availableSlotsForSameDay)
                            {
                                otherProfessors = availabilitiesList
                                    .Where(a => a.ProfessorId != team.AdvisorId && a.AvailableDate == advisorAvailability.AvailableDate && a.StartTime == slot.StartTime)
                                    .Select(a => a.ProfessorId)
                                    .Distinct()
                                    .Take(2)
                                    .ToList();

                                if (otherProfessors.Count >= 2)
                                {
                                    startTime = slot.StartTime;
                                    endTime = slot.StartTime + TimeSpan.FromMinutes(30);
                                    foundSuitableProfessors = true;
                                    break;
                                }
                            }

                            
                            if (!foundSuitableProfessors)
                            {
                                advisorAvailability = availabilitiesList
                                    .Where(a => a.ProfessorId == team.AdvisorId && a.AvailableDate > advisorAvailability.AvailableDate)
                                    .OrderBy(a => a.AvailableDate)
                                    .ThenBy(a => a.StartTime)
                                    .FirstOrDefault();

                                if (advisorAvailability == null)
                                {
                                    unassignedTeams.Add(team);
                                    break;
                                }

                                startTime = advisorAvailability.StartTime;
                                endTime = startTime + TimeSpan.FromMinutes(30);
                            }
                        }

                        
                        if (!foundSuitableProfessors && advisorAvailability == null)
                        {
                            unassignedTeams.Add(team);
                            break;
                        }
                    }

                   
                    if (foundSuitableProfessors)
                    {
                        
                        foreach (var professorId in otherProfessors)
                        {
                            var professorAvailability = availabilitiesList
                                 .Where(a => a.ProfessorId == professorId && a.AvailableDate.Date == advisorAvailability.AvailableDate.Date && a.StartTime == startTime && a.EndTime == endTime)
                                 .FirstOrDefault();

                            if (professorAvailability != null)
                            {
                                availabilitiesList.Remove(professorAvailability);
                            }
                        }

                        
                        var presentationDto = new TeamPresentationDTO
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

                        presentations.Add(presentationDto);
                    }
                }

                foreach (var presentationDto in presentations) 
                { 
                    await _teamPresentationAppService.AddTeamPresentationAsync(presentationDto);
                }

                
                return Ok(new
                {
                    message = "Scheduling completed.",
                    assignedTeams = presentations,
                    unassignedTeams = unassignedTeams,
                    availabilitiesList = availabilitiesList,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while scheduling teams.", error = ex.Message });
            }
        }


    }
}
