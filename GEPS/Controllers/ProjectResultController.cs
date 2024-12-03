using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using GEPS.Models;

namespace ProjectResult.Controllers
{
    [Route("ProjectResult")]
    public class ProjectResultController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProjectResultController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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

        [HttpGet("GetAllChecklistItems")]
        public async Task<IActionResult> GetAllChecklistItems()
        {
            string apiUrl = "https://localhost:7107/api/Admin/get-all-checklist-items";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var checklistItems = await response.Content.ReadFromJsonAsync<List<ChecklistItem>>();
                    return View("ChecklistItems", checklistItems);
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

        [HttpGet("Success")]
        public IActionResult Success()
        {
            ViewBag.Message = TempData["SuccessMessage"];
            return View();
        }



        [HttpGet("get-all-evaluation-criteria")]
        public async Task<IActionResult> GetAllEvaluationCriteria()
        {
            string apiUrl = "https://localhost:7107/api/Admin/get-all-evaluation-criteria";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var evaluationCriteriaList = await response.Content.ReadFromJsonAsync<List<EvaluationCriteria>>();
                    return View("EvaluationCriteriaList", evaluationCriteriaList); // Veriyi "EvaluationCriteriaList" view'a gönderiyoruz
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


    }
}
