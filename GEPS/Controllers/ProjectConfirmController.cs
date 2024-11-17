using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GEPS.Controllers
{
    public class ProjectConfirmController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProjectConfirmController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("ProjectConfirm/GetTeams")]
        public async Task<IActionResult> GetTeams()
        {
            string apiUrl = "https://localhost:7107/api/Professor/approval-teams-view";

            try
            {
                var teams = await _httpClient.GetFromJsonAsync<List<TeamResponse>>(apiUrl);

                if (teams == null || teams.Count == 0)
                {
                    return NotFound("Not Found Teams");
                }

                return Ok(teams);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"An error occurred during the API call: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("ProjectConfirm/ApproveOrRejectTeam")]
        public async Task<IActionResult> ApproveOrRejectTeam([FromBody] TeamApprovalRequest request)
        {
            string apiUrl = "https://localhost:7107/api/Professor/approval-teams?teamId=9&approval=true";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Team status updated successfully!" });
                }
                else
                {
                    return StatusCode((int)response.StatusCode, new { message = "Failed to update team status." });
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request.", error = ex.Message });
            }
        }
    }


    public class TeamResponse
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int ProjectId { get; set; }
        public int AdvisorId { get; set; }
        public bool IsActive { get; set; }
    }

    public class TeamApprovalRequest
    {
        public int TeamId { get; set; } 
        public bool IsApproved { get; set; } 
    }
}
