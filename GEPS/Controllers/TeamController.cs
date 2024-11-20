using GEPS.Models;
using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GEPS.Controllers
{
    public class TeamController : Controller
    {
        private readonly HttpClient _httpClient;

        public TeamController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("Team/TeamHome")]
        public async Task<IActionResult> TeamHome()
        {
            string apiUrl = "https://localhost:7107/api/WebStudent/projectTeamView";
            
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

        [HttpGet]
        [Route("Team/ProjectCreate")]
        public IActionResult ProjectCreate()
        {
            return View();
        }

        [HttpGet]
        [Route("Team/TeamHomeProfessor")]
        public async Task<IActionResult> TeamHomeProfessor()
        {
            string apiUrl = "https://localhost:7107/api/WebProfessor/projectTeamView";

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
