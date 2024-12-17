using GEPS.Filter;
using GEPS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace GEPS.Controllers
{
    [RoleFilter("Professor")]
    public class ProfessorController : Controller
    {

        private readonly HttpClient _httpClient;

        public ProfessorController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ************************************  Professor Profile Page  ************************************

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Professor/get-my-profile";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var professorProfile = await response.Content.ReadFromJsonAsync<Profile>();
                    return View(professorProfile);
                }
                else
                {
                    ViewBag.Errors = new[] { $"API call failed with status: {response.StatusCode}" };
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { $"An error occurred: {ex.Message}" };
                return View("Error");
            }
        }

        // ************************************ Get  Evaulation   ************************************

        [HttpGet("GetEvaluation/{evaluationId}")]
        public async Task<IActionResult> GetEvaluation(int evaluationId)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Professor/get-evaluation/{evaluationId}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var evaluationResult = await response.Content.ReadFromJsonAsync<ProjectEvaluationResult>();
                    return View("EvaluationResult", evaluationResult);
                }
                else
                {
                    ViewBag.Errors = new[] { $"API call failed with status: {response.StatusCode}" };
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { $"An error occurred: {ex.Message}" };
                return View("Error");
            }
        }

        // ************************************  Post Submit Evaluation   ************************************

        [HttpPost("SubmitEvaluation")]
        public async Task<IActionResult> SubmitEvaluation(ProjectEvaluationSubmit evaluationSubmitModel)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Professor/submit-evaluation";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, evaluationSubmitModel);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Evaluation submitted successfully!";
                    return RedirectToAction("Success");
                }
                else
                {
                    ViewBag.Errors = new[] { $"API call failed with status: {response.StatusCode}" };
                    return View("SubmitEvaluation", evaluationSubmitModel);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { $"An error occurred: {ex.Message}" };
                return View("SubmitEvaluation", evaluationSubmitModel);
            }
        }

        // ************************************ Get Project Team Result Evaluation   ************************************
        [HttpGet("GetProjectTeamResult/{teamId}")]
        public async Task<IActionResult> GetProjectTeamResult(int teamId)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Professor/get-project-team-result/{teamId}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var teamResult = await response.Content.ReadFromJsonAsync<ProjectTeamResult>();
                    return View("ProjectTeamResult", teamResult);
                }
                else
                {
                    ViewBag.Errors = new[] { $"API call failed with status: {response.StatusCode}" };
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { $"An error occurred: {ex.Message}" };
                return View("Error");
            }
        }

        // ************************************ Post Approval Team Evaluation   ************************************
        [HttpPost]
        public async Task<IActionResult> PostApprovalTeam(int teamId, bool approval)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = $"https://localhost:7107/api/Professor/post-approval-teams?teamId={teamId}&approval={approval}";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, new { TeamId = teamId, IsActive = approval });

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Team {teamId} approval status updated successfully!";
                    return RedirectToAction("GetApprovalTeams");
                }
                else
                {
                    ViewBag.Errors = new[] { $"API call failed with status: {response.StatusCode}" };
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { $"An error occurred: {ex.Message}" };
                return View("Error");
            }
        }

        // ************************************ Get Approval Teams View    ************************************

        [HttpGet]
        public async Task<IActionResult> getapprovalteamsview()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Professor/get-approval-teams-view";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var approvalTeams = await response.Content.ReadFromJsonAsync<List<ApprovalTeams>>();
                    return View(approvalTeams);
                }
                else
                {
                    ViewBag.Errors = new[] { $"API call failed with status: {response.StatusCode}" };
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { $"An error occurred: {ex.Message}" };
                return View("Error");
            }
        }

        // ************************************  Professor Home Page  ************************************

        [HttpGet]
        [Route("Professor/TeamHomeProfessor")]
        public async Task<IActionResult> TeamHomeProfessor()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Professor/get-project-team-view";

            try
            {
                string bearerToken = HttpContext.Session.GetString("BearerToken");

                if (string.IsNullOrEmpty(bearerToken))
                {
                    return Unauthorized("Bearer token is missing.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                var projectTeams = await _httpClient.GetFromJsonAsync<List<ProjectTeamResponse>>(apiUrl); 

                if (projectTeams == null || !projectTeams.Any())
                {
                    ViewBag.ErrorMessage = "No teams found.";
                    return View();
                }

                return View(projectTeams);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"An error occurred during the API call: {ex.Message}";
                return View();
            }
        }

        // ************************************   Teacher Approve Project Page ************************************

        [HttpPost]
        public async Task<IActionResult> TeacherApproveProject(int projectId, bool approval)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            try
            {
                string apiUrl = $"https://localhost:7107/api/Professor/approval-teams?teamId={projectId}&approval={approval}";

                var content = new StringContent(string.Empty); // Boş içerik
                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = approval
                        ? "Project approved successfully!"
                        : "Project rejected/deleted successfully!";
                }
                else
                {
                    ViewBag.Errors = new[] { $"API Error: {response.StatusCode}" };
                    return View("Error");
                }

                return RedirectToAction("TeacherApproveProject");
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { $"An error occurred: {ex.Message}" };
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> TeacherApproveProject()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Professor/get-project-team-view";
            try
            {
                string bearerToken = HttpContext.Session.GetString("BearerToken");

                if (string.IsNullOrEmpty(bearerToken))
                {
                    return Unauthorized("Bearer token is missing.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                var projectTeams = await _httpClient.GetFromJsonAsync<List<ProjectTeamResponse>>(apiUrl);

                if (projectTeams == null || !projectTeams.Any())
                {
                    ViewBag.ErrorMessage = "No teams found.";
                    return View();
                }

                return View(projectTeams);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"An error occurred during the API call: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> TeacherCalendar()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Professor/get-availability-by-professor-auth";

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
                    var professorAvailability = JsonConvert.DeserializeObject<List<ProfessorAvailability>>(content);

                    return View(professorAvailability);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = $"Değerlendirme kriterleri alınamadı. API Hatası: {response.StatusCode} - {errorContent}";
                    return View(new List<AdminEvaluationCriteria>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return View(new List<AdminEvaluationCriteria>());
            }
        }

        [HttpGet]
        public IActionResult CreateTeacherCalendar()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacherCalendar(ProfessorAvailability professorAvailability)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Professor/post-availability-by-professor";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, errorMessage = "Authorization token is missing." });
            }

            try
            {
                professorAvailability.ProfessorId = 0;  
                professorAvailability.AvailabilityId = 0; 

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(professorAvailability), Encoding.UTF8, "application/json")
                };

                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("TeacherCalendar");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, errorMessage = errorContent });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTeacherCalendar(int id)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            var apiUrl = $"https://localhost:7107/api/Professor/delete-availability-by-id/{id}";

            var token = HttpContext.Session.GetString("BearerToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Errors = new[] { "Authorization token is missing." };
                return View("Error");
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, apiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.SuccessMessage = "Tarih başarıyla silindi!";
                return RedirectToAction("TeacherCalendar");
            }
            else
            {
                ViewBag.ErrorMessage = "Tarih silinirken bir hata oluştu.";
                return RedirectToAction("TeacherCalendar");
            }
        }

        // *************************************   Teacher Evaluate Page  *************************************

        [HttpGet]
        public async Task<IActionResult> TeacherEvaluateProject()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Professor/get-project-team-view";

            try
            {
                string bearerToken = HttpContext.Session.GetString("BearerToken");

                if (string.IsNullOrEmpty(bearerToken))
                {
                    return Unauthorized("Bearer token is missing.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                var projectTeams = await _httpClient.GetFromJsonAsync<List<ProjectTeamResponse>>(apiUrl);

                if (projectTeams == null || !projectTeams.Any())
                {
                    ViewBag.ErrorMessage = "No teams found.";
                    return View();
                }

                return View(projectTeams);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"An error occurred during the API call: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostTeacherEvaluateProject(ProjectEvaluationSubmit evaluationModel)
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Professor/submit-evaluation";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, evaluationModel);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Evaluation submitted successfully!";
                    return RedirectToAction("TeamHomeProfessor"); 
                }
                else
                {
                    ViewBag.Errors = new[] { $"API Error: {response.StatusCode}" };
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                   
                ViewBag.Errors = new[] { $"An error occurred: {ex.Message}" };
                return View("Error");
            }
        }

        public async Task<IActionResult> TeacherViewResult()
        {
            var userRole = HttpContext.Items["UserRole"] as string;
            ViewBag.UserRole = userRole;

            string apiUrl = "https://localhost:7107/api/Professor/get-project-team-view";

            try
            {
                string bearerToken = HttpContext.Session.GetString("BearerToken");

                if (string.IsNullOrEmpty(bearerToken))
                {
                    return Unauthorized("Bearer token is missing.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                var projectTeams = await _httpClient.GetFromJsonAsync<List<ProjectTeamResponse>>(apiUrl);

                if (projectTeams == null || !projectTeams.Any())
                {
                    ViewBag.ErrorMessage = "No teams found.";
                    return View();
                }

                return View(projectTeams);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"An error occurred during the API call: {ex.Message}";
                return View();
            }
        }
    }
}
