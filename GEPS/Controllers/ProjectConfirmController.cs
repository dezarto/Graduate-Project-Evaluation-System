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
                    return NotFound("No teams found.");
                }

                return Ok(teams);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"An error occurred during the API call:{ex.Message}");
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
}
