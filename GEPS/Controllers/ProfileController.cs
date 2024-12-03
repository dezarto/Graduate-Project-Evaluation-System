using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GEPS.Models;

namespace GEPS.Controllers
{

        public class ProfileController : Controller
        {
            private readonly HttpClient _httpClient;

            public ProfileController(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

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
        }
    
}
