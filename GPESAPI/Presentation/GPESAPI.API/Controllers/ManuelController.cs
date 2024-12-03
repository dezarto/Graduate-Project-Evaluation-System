using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Application.Services;
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
        public async Task<IActionResult> ScheduleTeamsPresentationsOptimized()
        {
            try
            {
                // Fetch all team and professor availability data
                var teams = await _teamAppService.GetAllTeamAppAsync();
                var availabilities = await _professorAvailabilityAppService.GetAllProfessorAvailabilityAppAsync();
                var teamsList = teams.ToList();
                // Create a copy of the original availability list
                var availabilitiesListOriginal = availabilities.ToList();

                // List to store the best presentation schedule
                var presentationsResult = new List<TeamPresentationDTO>();

                // Initialize variables for tracking maximum matches and iterations
                int maxCount = 0;
                int now = 0;

                // Iterate through availability slots to maximize matches
                while (now < 30)
                {
                    var unassignedTeams = new List<TeamDTO>(); // List to track unmatched teams in this iteration
                    var presentations = new List<TeamPresentationDTO>(); // List to store presentations in this iteration
                    var availabilitiesList = availabilities.ToList(); // Copy of availability data for simulation

                    // Loop through each team to find a suitable schedule
                    foreach (var team in teams)
                    {
                        // Find the advisor's next available slot
                        var advisorAvailability = availabilitiesList
                            .Where(a => a.ProfessorId == team.AdvisorId)
                            .OrderBy(a => a.AvailableDate)
                            .ThenBy(a => a.StartTime)
                            .Skip(now)
                            .FirstOrDefault();

                        // If no valid availability or insufficient time, add to unassigned teams
                        if (advisorAvailability == null || advisorAvailability.EndTime < advisorAvailability.StartTime + TimeSpan.FromMinutes(30))
                        {
                            unassignedTeams.Add(team);
                            continue;
                        }

                        // Calculate presentation time slot
                        var startTime = advisorAvailability.StartTime;
                        var endTime = startTime + TimeSpan.FromMinutes(30);

                        // Find other professors available during the same time slot
                        var potentialProfessors = availabilitiesList
                            .Where(a => a.ProfessorId != team.AdvisorId && a.AvailableDate == advisorAvailability.AvailableDate)
                            .Where(a => a.StartTime <= startTime && a.EndTime >= endTime)
                            .GroupBy(a => a.ProfessorId)
                            .Select(g => g.First())
                            .ToList();

                        // If fewer than two professors are available, add to unassigned teams
                        if (potentialProfessors.Count < 2)
                        {
                            unassignedTeams.Add(team);
                            continue;
                        }

                        // Variables to store the best professor combination and maximum team matches
                        List<int> bestMatchingProfessors = null;
                        int maxTeamMatches = 0;

                        // Test all combinations of two professors for the best match
                        foreach (var professorCombination in potentialProfessors.Combinations(2))
                        {
                            // Simulate availability if the combination is chosen
                            var simulatedAvailabilities = new List<ProfessorAvailabilityDTO>(availabilitiesList);

                            foreach (var professor in professorCombination)
                            {
                                simulatedAvailabilities.RemoveAll(a => a.ProfessorId == professor.ProfessorId && a.AvailableDate == advisorAvailability.AvailableDate && a.StartTime == startTime && a.EndTime == endTime);
                            }

                            // Calculate how many teams can be matched with this simulation
                            var simulatedMatchCount = SimulateTeamMatches(teamsList, simulatedAvailabilities);

                            // Update the best combination if the current one is better
                            if (simulatedMatchCount > maxTeamMatches)
                            {
                                maxTeamMatches = simulatedMatchCount;
                                bestMatchingProfessors = professorCombination.Select(p => p.ProfessorId).ToList();
                            }
                        }

                        // If a valid combination is found, schedule the presentation
                        if (bestMatchingProfessors != null)
                        {
                            foreach (var professorId in bestMatchingProfessors)
                            {
                                availabilitiesList.RemoveAll(a => a.ProfessorId == professorId && a.AvailableDate == advisorAvailability.AvailableDate && a.StartTime == startTime && a.EndTime == endTime);
                            }

                            var presentationDto = new TeamPresentationDTO
                            {
                                TeamId = team.TeamId,
                                ProjectId = team.ProjectId,
                                AdvisorId = team.AdvisorId,
                                Professor1Id = bestMatchingProfessors[0],
                                Professor2Id = bestMatchingProfessors[1],
                                PresentationDate = advisorAvailability.AvailableDate,
                                StartTime = startTime,
                                EndTime = endTime
                            };

                            presentations.Add(presentationDto);
                        }
                        else
                        {
                            unassignedTeams.Add(team);
                        }
                    }

                    // Log the results of the current iteration
                    Console.WriteLine("Iteration: " + now + " Matched: " + presentations.Count + " Unmatched: " + unassignedTeams.Count);

                    // Update the best result if the current iteration has more matches
                    if (maxCount < presentations.Count)
                    {
                        presentationsResult.Clear();
                        presentationsResult = presentations;
                        maxCount = presentations.Count;
                    }

                    now++; // Move to the next availability slot
                }

                // Return the final scheduling results
                return Ok(new
                {
                    message = "Scheduling completed.",
                    presentationsResult = presentationsResult,
                    availabilitiesListOriginal = availabilitiesListOriginal,
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return StatusCode(500, new { message = "An error occurred while scheduling teams.", error = ex.Message });
            }
        }

        // Simulates team matches based on the given availability data
        private int SimulateTeamMatches(List<TeamDTO> teams, List<ProfessorAvailabilityDTO> availabilities)
        {
            int matchCount = 0; // Counter for matched teams

            foreach (var team in teams)
            {
                // Find the advisor's next available slot
                var advisorAvailability = availabilities
                    .Where(a => a.ProfessorId == team.AdvisorId)
                    .OrderBy(a => a.AvailableDate)
                    .ThenBy(a => a.StartTime)
                    .FirstOrDefault();

                // Skip if no valid availability or insufficient time
                if (advisorAvailability == null || advisorAvailability.EndTime < advisorAvailability.StartTime + TimeSpan.FromMinutes(30))
                    continue;

                // Calculate presentation time slot
                var startTime = advisorAvailability.StartTime;
                var endTime = startTime + TimeSpan.FromMinutes(30);

                // Find two other professors available during the same time slot
                var otherProfessors = availabilities
                    .Where(a => a.ProfessorId != team.AdvisorId && a.AvailableDate == advisorAvailability.AvailableDate)
                    .Where(a => a.StartTime <= startTime && a.EndTime >= endTime)
                    .Distinct()
                    .Take(2)
                    .ToList();

                // Increment match count if two professors are found
                if (otherProfessors.Count >= 2)
                    matchCount++;
            }

            return matchCount; // Return the total number of matches
        }


        [HttpPost("schedule-teams-presentations-optimized-test")]
        public async Task<IActionResult> ScheduleTeamsPresentationsOptimizedTest()
        {
            try
            {
                // Tüm takımları (projeleri) veri tabanından asenkron olarak alır.
                var teams = await _teamAppService.GetAllTeamAppAsync();

                // Tüm profesörlerin müsaitlik bilgilerini veri tabanından asenkron olarak alır.
                var availabilities = await _professorAvailabilityAppService.GetAllProfessorAvailabilityAppAsync();

                // Profesörlerin müsaitlik bilgilerini bir listeye dönüştürür (kopya olarak saklar).
                var availabilitiesListOriginal = availabilities.ToList();
                var presentationsResult = new List<TeamPresentationDTO>();
                int maxCount = 0; // En fazla eşleşen sunum sayısını tutar.
                int now = 0;      // Şu anki iterasyon numarasını tutar (max 30 döngü için).
                

                while (now < 30) // Maksimum 30 iterasyon boyunca işlem yapılır.
                {
                    // Eşleşemeyen takımları tutmak için bir liste.
                    var unassignedTeams = new List<TeamDTO>();

                    // Eşleşen sunumları tutmak için bir liste.
                    var presentations = new List<TeamPresentationDTO>();

                    // Profesörlerin müsaitlik bilgilerinin güncel bir kopyasını alır.
                    var availabilitiesList = availabilities.ToList();

                    foreach (var team in teams) // Her takım için döngü başlar.
                    {
                        // Takımın danışman hocasının müsaitlik bilgilerini filtreler, sıralar ve ilk uygun zamanı alır.
                        var advisorAvailability = availabilitiesList
                            .Where(a => a.ProfessorId == team.AdvisorId) // Danışman hocanın ID'sine göre filtreler.
                            .OrderBy(a => a.AvailableDate)               // Önce tarihe göre sıralar.
                            .ThenBy(a => a.StartTime)                   // Aynı tarihler arasında başlangıç saatine göre sıralar.
                            .Skip(now)                                  // İterasyona göre belirli kayıtları atlar.
                            .FirstOrDefault();                          // İlk uygun olan kaydı seçer.

                        if (advisorAvailability == null) // Eğer danışman hocanın uygun zamanı yoksa:
                        {
                            unassignedTeams.Add(team); // Takımı eşleşemeyenlere ekler.
                            continue; // Bir sonraki takıma geçer.
                        }
                        
                        // Sunumun başlangıç ve bitiş zamanlarını ayarlar (30 dakikalık sunum).
                        var startTime = advisorAvailability.StartTime;
                        var endTime = startTime + TimeSpan.FromMinutes(30);

                        // Eğer danışman hocanın zaman aralığı 30 dakikayı karşılamıyorsa:
                        if (advisorAvailability.EndTime < endTime)
                        {
                            unassignedTeams.Add(team); // Takımı eşleşemeyenlere ekler.
                            continue; // Bir sonraki takıma geçer.
                        }

                        List<int> otherProfessors = new List<int>(); // Diğer uygun profesörlerin ID'lerini tutmak için liste.
                        bool foundSuitableProfessors = false;       // İki uygun profesör bulunup bulunmadığını kontrol eder.

                        while (!foundSuitableProfessors) // İki uygun profesör bulunana kadar döner.
                        {
                            // Danışman hocanın dışında, aynı tarihte ve saat aralığında uygun olan profesörleri bulur.
                            otherProfessors = availabilitiesList
                                .Where(a => a.ProfessorId != team.AdvisorId && a.AvailableDate == advisorAvailability.AvailableDate)
                                .Where(a => a.StartTime <= startTime && a.EndTime >= endTime)
                                .Select(a => a.ProfessorId) // Sadece profesör ID'lerini seçer.
                                .Distinct() // Tekrar eden ID'leri kaldırır.
                                .Take(2) // Maksimum 2 profesör seçer.
                                .ToList();

                            if (otherProfessors.Count >= 2) // Eğer iki uygun profesör bulunursa:
                            {
                                foundSuitableProfessors = true; // Arama tamamlanır.
                            }
                            else // Eğer yeterli profesör bulunamazsa:
                            {
                                // Aynı gün içinde başka bir zaman aralığı arar.
                                var availableSlotsForSameDay = availabilitiesList
                                    .Where(a => a.ProfessorId != team.AdvisorId && a.AvailableDate == advisorAvailability.AvailableDate)
                                    .Where(a => a.StartTime > startTime && a.StartTime + TimeSpan.FromMinutes(30) <= a.EndTime)
                                    .Select(a => new { a.ProfessorId, a.StartTime, a.EndTime }) // Yeni zaman aralıklarını seçer.
                                    .ToList();

                                foreach (var slot in availableSlotsForSameDay) // Her uygun zaman aralığını kontrol eder.
                                {
                                    otherProfessors = availabilitiesList
                                        .Where(a => a.ProfessorId != team.AdvisorId && a.AvailableDate == advisorAvailability.AvailableDate && a.StartTime == slot.StartTime)
                                        .Select(a => a.ProfessorId) // Profesör ID'lerini seçer.
                                        .Distinct()
                                        .Take(2)
                                        .ToList();

                                    if (otherProfessors.Count >= 2) // Eğer iki uygun profesör bulunursa:
                                    {
                                        startTime = slot.StartTime; // Yeni başlangıç zamanını ayarlar.
                                        endTime = slot.StartTime + TimeSpan.FromMinutes(30); // Yeni bitiş zamanını ayarlar.
                                        foundSuitableProfessors = true;
                                        break; // Döngüyü sonlandırır.
                                    }
                                }

                                if (!foundSuitableProfessors) // Eğer uygun zaman aralığı bulunamazsa:
                                {
                                    advisorAvailability = availabilitiesList
                                        .Where(a => a.ProfessorId == team.AdvisorId && a.AvailableDate > advisorAvailability.AvailableDate)
                                        .OrderBy(a => a.AvailableDate)
                                        .ThenBy(a => a.StartTime)
                                        .FirstOrDefault(); // Danışman için bir sonraki uygun zamanı bulur.

                                    if (advisorAvailability == null) // Eğer yeni uygun zaman bulunamazsa:
                                    {
                                        unassignedTeams.Add(team); // Takımı eşleşemeyenlere ekler.
                                        break; // Döngüyü sonlandırır.
                                    }

                                    // Yeni başlangıç ve bitiş zamanlarını ayarlar.
                                    startTime = advisorAvailability.StartTime;
                                    endTime = startTime + TimeSpan.FromMinutes(30);
                                }
                            }

                            if (!foundSuitableProfessors && advisorAvailability == null)
                            {
                                unassignedTeams.Add(team); // Takımı eşleşemeyenlere ekler.
                                break;
                            }
                        }

                        if (foundSuitableProfessors) // Eğer uygun profesörler bulunduysa:
                        {
                            foreach (var professorId in otherProfessors) // Seçilen profesörleri müsaitlik listesinden kaldırır.
                            {
                                var professorAvailability = availabilitiesList
                                     .Where(a => a.ProfessorId == professorId && a.AvailableDate.Date == advisorAvailability.AvailableDate.Date && a.StartTime == startTime && a.EndTime == endTime)
                                     .FirstOrDefault();

                                if (professorAvailability != null)
                                {
                                    availabilitiesList.Remove(professorAvailability); // Zaman aralığını listeden çıkarır.
                                }
                            }

                            // Yeni bir sunum bilgisi oluşturur.
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

                            presentations.Add(presentationDto); // Sunumu listeye ekler.
                        }
                    }
                    Console.WriteLine("Asama: " + now + " Sonuc: " + presentations.Count + " Eslestirilemeyen: " + unassignedTeams.Count);
                    
                    if (maxCount < presentations.Count)
                    {
                        presentationsResult.Clear();
                        presentationsResult = presentations;
                        maxCount = presentations.Count; // En fazla eşleşen sunum sayısını günceller.
                    }
                    now++; // Bir sonraki iterasyona geçer.
                }

                return Ok(new
                {
                    message = "Scheduling completed.", // Planlama tamamlandı mesajı.
                    presentationsResult = presentationsResult,
                    availabilitiesListOriginal = availabilitiesListOriginal, // Orijinal müsaitlik listesi.
                });
            }
            catch (Exception ex)
            {
                // Bir hata oluşursa 500 hata kodu ve hata mesajı döndürür.
                return StatusCode(500, new { message = "An error occurred while scheduling teams.", error = ex.Message });
            }
        }

        [HttpPost("schedule-teams-presentations-optimized-backtracking")]
        public async Task<IActionResult> ScheduleTeamsPresentationsOptimizedBacktracking()
        {
            try
            {
                // Tüm takımları ve profesörlerin müsaitlik bilgilerini asenkron olarak veri tabanından alıyoruz.
                var teams = await _teamAppService.GetAllTeamAppAsync();
                var availabilities = await _professorAvailabilityAppService.GetAllProfessorAvailabilityAppAsync();

                // Tüm kombinasyonları denemek ve en iyi sonucu bulmak için backtracking algoritmasını kullanıyoruz.
                var availabilitiesListOriginal = availabilities.ToList(); // Orijinal müsaitlik listesi.
                var presentationsResult = new List<TeamPresentationDTO>(); // En iyi sonucu tutmak için liste.
                int maxCount = 0; // En fazla eşleşen sunum sayısını takip eder.

                // Backtracking çözümünü çağırıyoruz.
                if (AssignPresentationsBacktracking(teams.ToList(), availabilities.ToList(), new List<TeamPresentationDTO>(), ref presentationsResult, ref maxCount))
                {
                    return Ok(new
                    {
                        message = "Scheduling completed with backtracking.",
                        presentationsResult = presentationsResult,
                        availabilitiesListOriginal = availabilitiesListOriginal,
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = "No valid scheduling found after attempting all combinations.",
                        availabilitiesListOriginal = availabilitiesListOriginal,
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while scheduling teams.", error = ex.Message });
            }
        }

        // Backtracking algoritması
        private bool AssignPresentationsBacktracking(
            List<TeamDTO> teams,
            List<ProfessorAvailabilityDTO> availabilities,
            List<TeamPresentationDTO> currentPresentations,
            ref List<TeamPresentationDTO> bestResult,
            ref int maxCount)
        {
            if (!teams.Any()) // Eğer eşleştirilecek takım kalmadıysa
            {
                if (currentPresentations.Count > maxCount)
                {
                    maxCount = currentPresentations.Count;
                    bestResult = new List<TeamPresentationDTO>(currentPresentations); // En iyi sonucu güncelle.
                }
                return true;
            }

            var team = teams.First(); // İlk takımı seçiyoruz.
            var remainingTeams = teams.Skip(1).ToList(); // Kalan takımlar.

            // Takımın danışman hocasının uygun zamanlarını alıyoruz.
            var advisorAvailabilities = availabilities
                .Where(a => a.ProfessorId == team.AdvisorId)
                .OrderBy(a => a.AvailableDate)
                .ThenBy(a => a.StartTime)
                .ToList();

            foreach (var advisorAvailability in advisorAvailabilities)
            {
                var startTime = advisorAvailability.StartTime;
                var endTime = startTime + TimeSpan.FromMinutes(30);

                if (advisorAvailability.EndTime < endTime)
                    continue; // Eğer danışman hocanın zamanı yeterli değilse, sonraki zaman dilimine geç.

                // Aynı zamanda uygun diğer profesörleri buluyoruz.
                var otherProfessors = availabilities
                    .Where(a => a.ProfessorId != team.AdvisorId &&
                                a.AvailableDate == advisorAvailability.AvailableDate &&
                                a.StartTime <= startTime &&
                                a.EndTime >= endTime)
                    .Select(a => a.ProfessorId)
                    .Distinct()
                    .Take(2)
                    .ToList();

                if (otherProfessors.Count < 2)
                    continue; // Eğer yeterli sayıda profesör yoksa, sonraki zaman dilimine geç.

                // Seçilen zaman dilimini müsaitlik listesinden çıkar.
                availabilities.RemoveAll(a =>
                    (a.ProfessorId == team.AdvisorId || otherProfessors.Contains(a.ProfessorId)) &&
                    a.AvailableDate == advisorAvailability.AvailableDate &&
                    a.StartTime == startTime &&
                    a.EndTime >= endTime);

                // Yeni bir sunum bilgisi ekliyoruz.
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

                currentPresentations.Add(presentationDto);

                // Backtracking algoritmasını bir sonraki takım için çağırıyoruz.
                if (AssignPresentationsBacktracking(remainingTeams, availabilities, currentPresentations, ref bestResult, ref maxCount))
                    return true;

                // Eğer çözüm başarısız olursa, geri adım atıyoruz.
                currentPresentations.Remove(presentationDto);
                availabilities.Add(advisorAvailability); // Zaman dilimini geri ekle.
            }

            return false;
        }
    }
}
