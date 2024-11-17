using GEPS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // API URL'si
            string apiUrl = "https://localhost:7107/api/LoginCats";

            // API'ye gönderilecek veriler
            var loginData = new
            {
                Username = username,
                Password = password
            };

            try
            {
                // API'ye POST isteği gönder
                var response = await _httpClient.PostAsJsonAsync(apiUrl, loginData);

                if (response.IsSuccessStatusCode)
                {
                    // API yanıtını AuthResult olarak oku
                    var authResult = await response.Content.ReadFromJsonAsync<AuthResult>();

                    if (authResult != null && authResult.Success)
                    {
                        // Başarılıysa, HomePage'e yönlendir
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Başarısızsa, hata mesajlarını göster
                        ViewBag.Errors = authResult?.Errors ?? new[] { "Unknown error occurred." };
                        return View();
                    }
                }
                else
                {
                    // API yanıtı başarısızsa
                    ViewBag.Errors = new[] { "API call failed with status: " + response.StatusCode };
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj göster
                ViewBag.Errors = new[] { "An error occurred: " + ex.Message };
                return View();
            }
        }
    }

    // AuthResult sınıfı
    public class AuthResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
