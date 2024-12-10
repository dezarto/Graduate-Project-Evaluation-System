using GEPS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;

namespace GEPS.Controllers
{
    public class ProfessorController : Controller
    {

        private readonly HttpClient _httpClient;

        public ProfessorController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        // ********************************************************  Professor Profile Page  *************************************

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
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

        // ******************************************************** Get  Evaulation   *************************************
        [HttpGet("GetEvaluation/{evaluationId}")]
        public async Task<IActionResult> GetEvaluation(int evaluationId)
        {
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


        // ********************************************************  Post Submit Evaluation   *************************************
        [HttpPost("SubmitEvaluation")]
        public async Task<IActionResult> SubmitEvaluation(ProjectEvaluationSubmit evaluationSubmitModel)
        {
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


        // ******************************************************** Get Project Team Result Evaluation   *************************************
        [HttpGet("GetProjectTeamResult/{teamId}")]
        public async Task<IActionResult> GetProjectTeamResult(int teamId)
        {
            string apiUrl = $"https://localhost:7107/api/Professor/get-project-team-result/{teamId}";

            try
            {
                // GET isteği gönder
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // JSON yanıtını modele dönüştür
                    var teamResult = await response.Content.ReadFromJsonAsync<ProjectTeamResult>();
                    return View("ProjectTeamResult", teamResult); // "ProjectTeamResult" bir view olacak
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

        // ******************************************************** Post Approval Team Evaluation   *************************************
        [HttpPost]
        public async Task<IActionResult> PostApprovalTeam(int teamId, bool approval)
        {
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



        // ******************************************************** Get Approval Teams View    *************************************

        [HttpGet]
        public async Task<IActionResult> getapprovalteamsview()
        {
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

        // ********************************************************  Professor Home Page  *************************************

        [HttpGet]
        [Route("Professor/TeamHomeProfessor")]
        public async Task<IActionResult> TeamHomeProfessor()
        {
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

        // ********************************************************   Teacher Approve Project Page (ApproveRejectProject bu kısma dahildir.)  *************************************


        [HttpGet]
        public async Task<IActionResult> TeacherApproveProject()
        {
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
        public async Task<IActionResult> ApproveRejectProject(int teamId, bool approval)
        {
            string apiUrl = $"https://localhost:7107/api/Professor/post-approval-teams?teamId={teamId}&approval={approval}";
            try
            {
                var response = await _httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = approval ? "Project approved successfully!" : "Project rejected successfully!";
                    return RedirectToAction("TeacherApproveProject");
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






        public IActionResult TeacherCalendar()
        {
            return View();
        }

        

        // ********************************************************   Teacher Evaluate Page  *************************************

        [HttpGet]
        public async Task<IActionResult> TeacherEvaluateProject()
        {
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
