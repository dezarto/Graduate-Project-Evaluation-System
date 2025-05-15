using GEPS.Filter;
using GEPS.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

namespace GEPS.Controllers
{
    [RoleFilter("Student")]
    public class StudentController : Controller
    {
        private readonly HttpClient _httpClient;

        public StudentController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ************************************   Student Go to TeamHome (Home page)   ************************************
        [HttpGet]
        [Route("Student/TeamHome")]
        public async Task<IActionResult> TeamHome()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

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

                if (projectTeam.TeamId == null)
                {
                    var newEmptyList = new StudentProjectTeamsWeb
                    {
                        TeamId = 0,
                        AdvisorId = 0,
                        Description = string.Empty,
                        isActive = false,
                        Members = new List<MemberList>(),
                        ProjectId = 0,
                        ProjectName = string.Empty,
                        TeamName = string.Empty,
                        ReportId = 0,
                    };

                    return View(newEmptyList);
                }

                return View(projectTeam);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"An error occurred during the API call: {ex.Message}";
                return View();
            }
        }

        // ************************************   Student Create Project Topics    ************************************

        [HttpGet]
        [Route("Student/ProjectCreate")]
        public async Task<IActionResult> ProjectCreate()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Student/get-all-professor";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var professorList = JsonConvert.DeserializeObject<List<Professor>>(content);

                    // Profesör listesini ViewBag ile gönderiyoruz
                    ViewBag.ProfessorList = professorList ?? new List<Professor>();
                    return View();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Profesör bilgileri alınamadı. API Hatası: {response.StatusCode} - {errorContent}";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(TeamCreator teamCreator)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Student/create-team";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(teamCreator), Encoding.UTF8, "application/json")
                };

                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Project submitted successfully!";
                    return RedirectToAction("TeamHome");
                }
                else
                {
                    ViewBag.Errors = new[] { "API call failed with status: " + response.StatusCode };
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { "An error occurred: " + ex.Message };
                return View("Error");
            }
        }

        // ************************************   Student Upload Project    ************************************

        [HttpGet]
        public IActionResult ProjectUpload()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            return View();
        }

        [HttpPost("project-upload")]
        public async Task<IActionResult> ProjectUpload([FromForm] ProjectUpload request)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            
            try
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var objectId = ObjectId.GenerateNewId().ToString();
                var fileExtension = Path.GetExtension(request.File.FileName);
                var fullName = objectId + fileExtension;
                var filePath = Path.Combine(uploadPath, fullName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }

                var apiUrl = $"https://localhost:7107/api/Student/project-upload/{Uri.EscapeDataString(fullName)}";

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);

                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Project submitted successfully!";
                    return RedirectToAction("TeamHome");
                }
                else
                {
                    ViewBag.Errors = new[] { "API call failed with status: " + response.StatusCode };
                    return View(filePath);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during the file upload.", error = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
