using GEPS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GEPS.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        // Admin.cs içierisinde yer alan AdminChecklistItem modelini kullanır. 
        [HttpGet("get-all-checklist-items")]
        public async Task<IActionResult> GetAllChecklistItems()
        {
            string apiUrl = "https://localhost:7107/api/Admin/get-all-checklist-items";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var checklistItems = await response.Content.ReadFromJsonAsync<List<AdminChecklistItem>>();

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


        // GET: /Admin/GetAllEvaluationCriteria
        //Admin.cs içierisinde yer alan AdminEvaluationCriteria modelini kullanır.
        [HttpGet("get-all-evaluation-criteria")]
        public async Task<IActionResult> GetAllEvaluationCriteria()
        {
            string apiUrl = "https://localhost:7107/api/Admin/get-all-evaluation-criteria";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var evaluationCriteria = await response.Content.ReadFromJsonAsync<List<AdminEvaluationCriteria>>();

                    return View("EvaluationCriteria", evaluationCriteria);
                }
                else
                {
                    ViewBag.Errors = new[] { $"API call failed with status: {response.StatusCode}" };
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ViewBag.Errors = new[] { $"An error occurred: {ex.Message}" };
                return View("Error");
            }
        }
    }
}
