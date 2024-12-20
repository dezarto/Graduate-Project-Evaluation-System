using GEPS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("LogoutSystem")]
        public IActionResult LogoutSystem()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
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
                        TempData["Token"] = authResult.Token;

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
