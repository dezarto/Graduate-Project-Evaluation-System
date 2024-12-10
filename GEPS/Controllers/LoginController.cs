using GEPS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GEPS.Controllers
{
    [Route("Login")]
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            string apiUrl = "https://localhost:7107/api/LoginCats";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, login);

                if (response.IsSuccessStatusCode)
                {
                    var authResult = await response.Content.ReadFromJsonAsync<AuthResult>();

                    if (authResult != null && authResult.Success)
                    {
                        HttpContext.Session.SetString("BearerToken", authResult.Token);
                        if (login.Username.Contains("@iku.edu.tr")) 
                        {
                            return RedirectToAction("TeamHomeProfessor", "Professor");
                        }
                        else
                        {
                            return RedirectToAction("TeamHome", "Student");
                        }
                    }
                    else
                    {
                        ViewBag.Errors = authResult?.Errors ?? new[] { "Unknown error occurred." };
                        return View();
                    }
                }
                else
                {
                    ViewBag.Errors = new[] { "API call failed with status: " + response.StatusCode };
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { "An error occurred: " + ex.Message };
                return View();
            }
        }

    }
}
