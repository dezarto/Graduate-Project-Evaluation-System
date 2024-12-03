using GEPS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace GEPS.Controllers
{
    public class TeamApprovalController : Controller
    {
        private readonly HttpClient _httpClient;

        public TeamApprovalController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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


        [HttpGet]
        public async Task<IActionResult> Index()
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
    }
}
