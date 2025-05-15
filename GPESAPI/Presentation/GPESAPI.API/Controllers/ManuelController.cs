using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("schedule-teams-presentations-optimized-backtracking")]
        public async Task<IActionResult> ScheduleTeamsPresentationsOptimizedBacktracking()
        {
            try
            {
                // Tüm takımları ve profesörlerin müsaitlik bilgilerini al
                var teams = await _teamAppService.GetAllTeamAppAsync();
                var availabilities = await _professorAvailabilityAppService.GetAllProfessorAvailabilityAppAsync();

                var ListOfTeams = teams.ToList();
                var availabilitiesListOriginal = availabilities.ToList(); // Orijinal müsaitlik listesi
                var presentationsResult = new List<TeamPresentationDTO>(); // En iyi sonucu tutmak için liste
                var unassignedTeams = new List<TeamDTO>();
                
                int maxCount = 0; 
                var currentIndex = 0;
                var sonKont = false;

                Console.WriteLine("********************** İlk Deneme ***********************************************************");
                if (AssignPresentationsBacktracking(teams.ToList(),
                    availabilities.ToList(),
                    new List<TeamPresentationDTO>(),
                    ref presentationsResult,
                    ref maxCount,
                    ref currentIndex,
                    ref sonKont))
                {
                    foreach (var item in presentationsResult)
                    {
                        await _teamPresentationAppService.AddTeamPresentationAsync(item);
                    }

                    return Ok(new
                    {
                        message = "Scheduling completed with backtracking.",
                        presentationsResult = presentationsResult,
                        availabilitiesListOriginal = availabilitiesListOriginal,
                    });
                }
                var a = 0;
                while (true)
                {
                    a++;
                    var teamRemove = ListOfTeams.FirstOrDefault(t => t.TeamId == currentIndex);
                    sonKont = false;
                    if (teamRemove != null)
                    {
                        unassignedTeams.Add(teamRemove);
                        ListOfTeams.Remove(teamRemove);
                    }

                    // Tekrar çağırdık
                    Console.WriteLine("********************** Tekrar Deneniyor  ***********************************************************");
                    if (AssignPresentationsBacktracking(ListOfTeams,
                    availabilities.ToList(),
                    new List<TeamPresentationDTO>(),
                    ref presentationsResult,
                    ref maxCount,
                    ref currentIndex,
                    ref sonKont))
                    {
                        foreach (var item in presentationsResult)
                        {
                            await _teamPresentationAppService.AddTeamPresentationAsync(item);
                        }

                        return Ok(new
                        {
                            message = "Scheduling completed with backtracking.",
                            unassignedTeams = unassignedTeams,
                            presentationsResult = presentationsResult
                        });
                    }
                    else
                    {
                        Console.WriteLine("****************************************************************** Deneme Sayısı:" + a);
                    }
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
            ref int maxCount,
            ref int currentIndex,
            ref bool sonKont)
        {
            // Eğer eşleştirilecek takım kalmadıysa
            if (!teams.Any())
            {
                if (currentPresentations.Count > maxCount)
                {
                    maxCount = currentPresentations.Count;
                    Console.WriteLine("Max Count Güncellendi takım kalmadı: " + maxCount);
                    bestResult = new List<TeamPresentationDTO>(currentPresentations); // En iyi sonucu güncelle.
                    Console.WriteLine("Best result ayarlandı...");
                }
                if(currentPresentations.Count == maxCount)
                {
                    bestResult = new List<TeamPresentationDTO>(currentPresentations);
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
            
            var b = 0;

            int currentIndexFor = 0;
            int lastIndex = advisorAvailabilities.Count - 1;
            var sonMu = false;

            foreach (var advisorAvailability in advisorAvailabilities)
            {
                if (currentIndexFor == lastIndex)
                {
                    Console.WriteLine("Foreach döngüsünün son adımındasınız.");
                    sonMu = true;
                }
                currentIndexFor++;

                b++;
                Console.WriteLine("Takım: " + team.TeamId + " Toplam ZHM: " + advisorAvailabilities.Count + " Şu an denene ZHM: " + b);
                var startTime = advisorAvailability.StartTime;
                var endTime = startTime + TimeSpan.FromMinutes(30);

                if (advisorAvailability.EndTime < endTime)
                {
                    if (sonMu && sonKont == false)
                    {
                        currentIndex = team.TeamId;
                        sonKont = true;
                    }
                    Console.WriteLine("Bulamadım - Profesör müsait saatinin bitişi uyuşmadı");
                    continue; // Eğer danışman hocanın zamanı yeterli değilse, sonraki zaman dilimine geç.
                }

                // Aynı zamanda uygun diğer profesörleri buluyoruz.
                var otherProfessors = availabilities
                    .Where(a => a.ProfessorId != team.AdvisorId &&
                                a.AvailableDate == advisorAvailability.AvailableDate &&
                                a.StartTime <= startTime &&
                                a.EndTime >= endTime)
                    .Select(a => a.ProfessorId)
                    .Distinct()
                    .ToList();

                if (otherProfessors.Count < 2)
                {
                    if (sonMu && sonKont == false) 
                    {
                        currentIndex = team.TeamId;
                        sonKont=true;
                    }
                    Console.WriteLine("Bulamadım - Profesör sayısı yeteersiz");
                    
                    continue; // Eğer yeterli sayıda profesör yoksa, sonraki zaman dilimine geç.
                }

                // Kombinasyon
                Console.WriteLine("Kombinasyon alma çağrılıyor...");
                var combinations = GetCombinations(otherProfessors.ToArray(), 2); // 2 li kombinasyonlarını alıyor
                var a = 0;

                foreach (var combination in combinations)
                {
                    a++;
                    Console.WriteLine("Takım: " + team.TeamId + " Toplam Komb: " + combinations.Length + " Şu an denene komb: " + a);
                    // Seçilen zaman dilimini müsaitlik listesinden çıkar.
                    availabilities.RemoveAll(a =>
                        (a.ProfessorId == team.AdvisorId || combination.Contains(a.ProfessorId)) &&
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

                    if(currentPresentations.Count > maxCount)
                    {
                        maxCount = currentPresentations.Count;
                        Console.WriteLine("Max Count Güncellendi... : " + maxCount);
                    }

                    Console.WriteLine("Başarılı - Sonraki takıma geçiliyor...");

                    if (currentIndex == team.TeamId)
                    {
                        sonMu = false;
                        sonKont = false;
                        currentIndex = 0;
                    }

                    // Backtracking algoritmasını bir sonraki takım için çağırıyoruz.
                    if (AssignPresentationsBacktracking(remainingTeams, availabilities, currentPresentations, ref bestResult, ref maxCount, ref currentIndex, ref sonKont))
                        return true;

                    // Eğer çözüm başarısız olursa, geri adım atıyoruz.
                    currentPresentations.Remove(presentationDto);
                    availabilities.Add(advisorAvailability); // Zaman dilimini geri ekle.

                    foreach (var professorId in combination)
                    {
                        var professorAvailability = new ProfessorAvailabilityDTO
                        {
                            ProfessorId = professorId,
                            AvailableDate = advisorAvailability.AvailableDate,
                            StartTime = startTime,
                            EndTime = advisorAvailability.EndTime
                        };
                        availabilities.Add(professorAvailability);
                    }
                    
                    Console.WriteLine("Başarısız bulamadı!" + "Takım: " + team.TeamId + " Toplam ZHM: " + advisorAvailabilities.Count + " Şu an denene ZHM: " + b + " Toplam Komb: " + combinations.Length + " Şu an denene komb: " + a);
                }
            }

            return false;
        }


        // Kombinasyonları hesaplayan metod
        static T[][] GetCombinations<T>(T[] array, int r)
        {
            int n = array.Length;
            var result = new List<T[]>();

            // Kombinasyonları oluşturmak için rekürsif fonksiyon
            GenerateCombinations(array, r, 0, new T[r], 0, result);

            return result.ToArray();
        }

        static void GenerateCombinations<T>(T[] array, int r, int index, T[] current, int start, List<T[]> result)
        {
            if (index == r)
            {
                result.Add((T[])current.Clone());
                return;
            }

            for (int i = start; i < array.Length; i++)
            {
                current[index] = array[i];
                GenerateCombinations(array, r, index + 1, current, i + 1, result);
            }
        }
    }
}
