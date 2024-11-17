using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GEPS.Controllers
{
    public class TeamsAndProjectsController : Controller
    {
        private readonly HttpClient _httpClient;

        public TeamsAndProjectsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult SubmitProject()
        {
            return View(new TeamCreator
            {
                StudentList = new List<StudenLists>
                {
                    new StudenLists(), // Varsayılan bir öğrenci alanı ekleniyor
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitProject(TeamCreator teamCreator)
        {
            // API URL'si
            string apiUrl = "https://localhost:7107/api/Team/createTeam";

            try
            {
                // API'ye POST isteği gönder
                var response = await _httpClient.PostAsJsonAsync(apiUrl, teamCreator);

                if (response.IsSuccessStatusCode)
                {
                    // API başarılı bir şekilde 200 OK döndürdüyse
                    TempData["SuccessMessage"] = "Project submitted successfully!";
                    return RedirectToAction("Success");
                }
                else
                {
                    // API yanıtı başarısızsa
                    ViewBag.Errors = new[] { "API call failed with status: " + response.StatusCode };
                    return View(teamCreator);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj göster
                ViewBag.Errors = new[] { "An error occurred: " + ex.Message };
                return View(teamCreator);
            }
        }

        [HttpGet]
        public IActionResult Success()
        {
            ViewBag.Message = TempData["SuccessMessage"];
            return View();
        }
    }

    public class TeamCreator
    {
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string TeamName { get; set; }
        public List<StudenLists> StudentList { get; set; }
    }


    public class StudenLists
    {
        public string StudentFullName { get; set; }
        public string StudenNumber { get; set; }
    }
}
