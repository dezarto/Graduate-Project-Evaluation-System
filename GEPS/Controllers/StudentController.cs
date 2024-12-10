using GEPS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GEPS.Controllers
{
    public class StudentController : Controller
    {
        private readonly HttpClient _httpClient;

        public StudentController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // **********************************************************   Student Create Team   **********************************************************


        [HttpPost]
        public async Task<IActionResult> CreateTeam(TeamCreator teamCreator)
        {
            // API URL'si
            string apiUrl = "https://localhost:7107/api/Student/create-team";

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

        // **********************************************************   Student Go to TeamHome (Home page)   **********************************************************
        [HttpGet]
        [Route("Student/TeamHome")]
        public async Task<IActionResult> TeamHome()
        {
            string apiUrl = "https://localhost:7107/api/Student/project-team-view";

            try
            {
                string bearerToken = HttpContext.Session.GetString("BearerToken");

                if (string.IsNullOrEmpty(bearerToken))
                {
                    return Unauthorized("Bearer token is missing.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                var projectTeam = await _httpClient.GetFromJsonAsync<StudentProjectTeamsWeb>(apiUrl);


                if (projectTeam == null)
                {
                    ViewBag.ErrorMessage = "No teams found.";
                    return View();
                }

                return View(projectTeam);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"An error occurred during the API call: {ex.Message}";
                return View();
            }
        }


        // **********************************************************   Student Create Project Topics    **********************************************************

        [HttpGet]
        [Route("Student/ProjectCreate")]
        public IActionResult ProjectCreate()
        {
            var newTeamCreator = new TeamCreator
            {
                AdvisorName = "",
                Description = "",
                ProjectName = "",
                StudentList = new List<StudenLists>
                {
                    new StudenLists { StudenNumber = "", StudentFullName = "" },
                    new StudenLists { StudenNumber = "", StudentFullName = "" },
                    new StudenLists { StudenNumber = "", StudentFullName = "" }
                },
                TeamName = ""
            };

            return View(newTeamCreator);
        }



        public IActionResult Index()
        {
            return View();
        }


        // **********************************************************   Student Upload Project    **********************************************************

        [HttpPost]
        public async Task<IActionResult> ProjectUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a file to upload.";
                return View();
            }

            string apiUrl = "https://localhost:7107/api/Student/project-upload";

            try
            {
                // HTTP istemci için istek mesajı oluştur
                using (var formContent = new MultipartFormDataContent())
                {
                    // Dosyayı form içeriğine ekle
                    using (var fileStream = file.OpenReadStream())
                    {
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                        formContent.Add(fileContent, "file", file.FileName);
                    }

                    // Token'ı header'a ekle
                    string token = HttpContext.Session.GetString("AuthToken"); // Token'ın burada saklandığını varsayıyoruz
                    if (!string.IsNullOrEmpty(token))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }

                    // API'ye POST isteği gönder
                    var response = await _httpClient.PostAsync(apiUrl, formContent);

                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "File uploaded successfully!";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = $"File upload failed. Status code: {response.StatusCode}";
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"An error occurred: {ex.Message}";
                return View("Error");
            }
        }




        //Display project Topics sayfası olacak mı ??

        //public IActionResult DisplayProjectTopics()
        //{
        //    return View();
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
