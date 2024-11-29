using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace GPESAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Professor")]
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

        [HttpPost("sync-users-with-professors")]
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

        [HttpPost("schedule-teams-presentations")]
        public async Task<IActionResult> ScheduleTeamsPresentations()
        {
            try
            {
                var teams = await _teamAppService.GetAllTeamAppAsync(); 
                var availabilities = await _professorAvailabilityAppService.GetAllProfessorAvailabilityAppAsync(); 
                var presentations = new List<TeamPresentationDTO>();
                var unassignedTeams = new List<TeamDTO>(); 

                var availabilitiesList = availabilities.ToList();
                var availabilitiesListOriginal = availabilitiesList;
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
                    //await _teamPresentationAppService.AddTeamPresentationAsync(presentationDto);
                }

                return Ok(new
                {
                    message = "Scheduling completed.",
                    assignedTeams = presentations,
                    unassignedTeams = unassignedTeams,
                    availabilitiesList = availabilitiesList,
                    availabilitiesListOriginal = availabilitiesListOriginal,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while scheduling teams.", error = ex.Message });
            }
        }

        [HttpPost("schedule-teams-presentations-optimized")]
        public async Task<IActionResult> ScheduleTeamsPresentationsop()
        {
            try
            {
                var teams = await _teamAppService.GetAllTeamAppAsync();
                var availabilities = await _professorAvailabilityAppService.GetAllProfessorAvailabilityAppAsync();
                var presentations = new List<TeamPresentationDTO>();
                var unassignedTeams = new List<TeamDTO>();

                var availabilitiesList = availabilities.ToList();
                var availabilitiesListOriginal = availabilitiesList;

                int initialMatchCount = 0; // İlk eşleştirme sayısını tutacak değişken
                int secondMatchCount = 0; // İkinci eşleştirme sayısını tutacak değişken
                int[] arr = new int[20];

                // İlk eşleştirmeyi yapıyoruz
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
                            //initialMatchCount++; // İlk eşleştirme sayısını artırıyoruz
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
                                    //initialMatchCount++; // İlk eşleştirme sayısını artırıyoruz
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

                // İkinci eşleştirme işlemi: Danışmanın bir sonraki müsaitlik zamanına göre
                availabilitiesList = availabilitiesListOriginal.ToList(); // Orijinal listeyi geri alıyoruz
                var secondPresentations = new List<TeamPresentationDTO>();
                foreach (var team in teams)
                {
                    var advisorAvailability = availabilitiesList
                        .Where(a => a.ProfessorId == team.AdvisorId)
                        .OrderBy(a => a.AvailableDate)
                        .ThenBy(a => a.StartTime)
                        .Skip(1) // İlk müsaitlik zamanını atlıyoruz
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
                            secondMatchCount++; // İkinci eşleştirme sayısını artırıyoruz
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
                                    secondMatchCount++; // Eşleştirme sayısını artırıyoruz
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

                        secondPresentations.Add(presentationDto);
                    }
                }

                // Perform the third matching phase if necessary, based on counts (initialMatchCount, secondMatchCount)
                if (initialMatchCount + secondMatchCount > 6)
                {
                    Console.WriteLine("Third Phase Triggered");

                    var remainingTeams = unassignedTeams; // Teams that couldn't be assigned in the first two phases
                    var thirdPhaseAssignments = new List<TeamPresentationDTO>();

                    // We will try to assign remaining unassigned teams
                    foreach (var team in remainingTeams)
                    {
                        var advisorAvailability = availabilitiesList
                            .Where(a => a.ProfessorId == team.AdvisorId)
                            .OrderBy(a => a.AvailableDate)
                            .ThenBy(a => a.StartTime)
                            .FirstOrDefault();

                        if (advisorAvailability == null)
                        {
                            // If no availability for the advisor, skip this team
                            continue;
                        }

                        var startTime = advisorAvailability.StartTime;
                        var endTime = startTime + TimeSpan.FromMinutes(30);

                        if (advisorAvailability.EndTime < endTime)
                        {
                            // If advisor's time is not available for the entire presentation duration, skip this team
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
                                .Take(2) // Try to find 2 other professors
                                .ToList();

                            if (otherProfessors.Count >= 2)
                            {
                                // Professors found for this slot
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
                                    // Try to find a new advisor availability if the current one doesn't work
                                    advisorAvailability = availabilitiesList
                                        .Where(a => a.ProfessorId == team.AdvisorId && a.AvailableDate > advisorAvailability.AvailableDate)
                                        .OrderBy(a => a.AvailableDate)
                                        .ThenBy(a => a.StartTime)
                                        .FirstOrDefault();

                                    if (advisorAvailability == null)
                                    {
                                        // No more availability for the advisor, skip this team
                                        break;
                                    }

                                    startTime = advisorAvailability.StartTime;
                                    endTime = startTime + TimeSpan.FromMinutes(30);
                                }
                            }

                            if (!foundSuitableProfessors && advisorAvailability == null)
                            {
                                // If we couldn't find any professors and advisor availability is null, skip this team
                                break;
                            }
                        }

                        if (foundSuitableProfessors)
                        {
                            // Remove professors' availability after assigning them
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

                            // Create the presentation assignment for the third phase
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

                            thirdPhaseAssignments.Add(presentationDto);
                        }
                    }

                    // If there are third-phase assignments, update the presentations list
                    if (thirdPhaseAssignments.Any())
                    {
                        presentations.AddRange(thirdPhaseAssignments);
                        Console.WriteLine("Third Phase Assignments Completed", presentations.Count);
                    }
                    else
                    {
                        Console.WriteLine("No suitable third-phase assignments found");
                    }
                }


                return Ok(new { presentations, unassignedTeams });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }




        [HttpPost("schedule-teams-presentations-optimized-test")]
        public async Task<IActionResult> ScheduleTeamsPresentationsOptimizedTest()
        {
            try
            {
                var teams = await _teamAppService.GetAllTeamAppAsync();
                var availabilities = await _professorAvailabilityAppService.GetAllProfessorAvailabilityAppAsync();
                
                

                var availabilitiesListOriginal = availabilities.ToList();

                int maxCount = 0;
                int now = 0;

                while (now < 30) 
                {
                    var unassignedTeams = new List<TeamDTO>();
                    var presentations = new List<TeamPresentationDTO>();
                    var availabilitiesList = availabilities.ToList();
                    foreach (var team in teams)
                    {
                        var advisorAvailability = availabilitiesList
                            .Where(a => a.ProfessorId == team.AdvisorId)
                            .OrderBy(a => a.AvailableDate)
                            .ThenBy(a => a.StartTime)
                            .Skip(now)
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
                                    .Where(a => a.StartTime >= startTime && a.StartTime + TimeSpan.FromMinutes(30) <= a.EndTime)
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
                    Console.WriteLine("Asama: " + now + " Sonuc: " + presentations.Count + " Eslestirilemeyen: " + unassignedTeams.Count);

                    if (maxCount < presentations.Count)
                    {
                        maxCount = presentations.Count;
                    }
                    now++;
                }

                return Ok(new
                {
                    message = "Scheduling completed.",
                    //assignedTeams = presentations,
                   // unassignedTeams = unassignedTeams,
                    availabilitiesListOriginal = availabilitiesListOriginal,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while scheduling teams.", error = ex.Message });
            }
        }

    }
}
