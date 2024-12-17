using GEPS.Filter;
using GEPS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

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

        // ************************************  Student Create Team   ************************************

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
                    Content = JsonContent.Create(teamCreator)
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
                    return View(teamCreator);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { "An error occurred: " + ex.Message };
                return View(teamCreator);
            }
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

                    if(professorList != null)
                    {    
                        var newTeamCreator = new TeamCreator
                        {
                            Description = "",
                            ProjectName = "",
                            StudentList = new List<StudenLists>
                            {
                                new StudenLists { StudenNumber = "", StudentFullName = "" },
                                new StudenLists { StudenNumber = "", StudentFullName = "" },
                                new StudenLists { StudenNumber = "", StudentFullName = "" }
                            },
                            ProfessorList = professorList ?? new List<Professor>(),
                            TeamName = "",
                            SelectedProfessorId = null
                        };
                        return View(newTeamCreator);
                    }
                    else
                    {
                        var newTeamCreator = new TeamCreator
                        {
                            Description = "",
                            ProjectName = "",
                            StudentList = new List<StudenLists>
                            {
                                new StudenLists { StudenNumber = "", StudentFullName = "" },
                                new StudenLists { StudenNumber = "", StudentFullName = "" },
                                new StudenLists { StudenNumber = "", StudentFullName = "" }
                            },
                            ProfessorList = new List<Professor>(),
                            TeamName = "",
                            SelectedProfessorId = null
                        };
                        return View(newTeamCreator);
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Profesör bilgileri alınamadı. API Hatası: {response.StatusCode} - {errorContent}";
                    return View(new List<Professor>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return View(new List<Professor>());
            }
        }

        // ************************************   Student Upload Project    ************************************

        [HttpPost]
        public async Task<IActionResult> ProjectUpload(IFormFile file)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a file to upload.";
                return View();
            }

            string apiUrl = "https://localhost:7107/api/Student/project-upload";

            try
            {
                using (var formContent = new MultipartFormDataContent())
                {
                    using (var fileStream = file.OpenReadStream())
                    {
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                        formContent.Add(fileContent, "file", file.FileName);
                    }

                    string token = HttpContext.Session.GetString("AuthToken"); // Token'ın burada saklandığını varsayıyoruz
                    if (!string.IsNullOrEmpty(token))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }

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
