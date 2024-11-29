using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;


namespace GPESAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginOrionController : Controller
    {
        private readonly ITokenService _tokenService;

        public LoginOrionController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            try
            {
                ChromeOptions options = new ChromeOptions();
                //options.AddArgument("--headless");

                using (var driver = new ChromeDriver(options))
                {
                    driver.Navigate().GoToUrl("https://orion.iku.edu.tr/");

                    var usernameInput = driver.FindElement(By.Id("logonuidfield"));
                    var passwordInput = driver.FindElement(By.Id("logonpassfield"));

                    usernameInput.SendKeys(request.Username);
                    passwordInput.SendKeys(request.Password);

                    var loginButton = driver.FindElement(By.ClassName("button"));
                    loginButton.Click();

                    var information = driver.FindElement(By.Id("__tile7"));
                    information.Click();

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
                    wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
                    // İstediğiniz ID'deki veriyi almak
                    var advisorTeacher = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("__text116")));

                    // Veriyi alma
                    string data = advisorTeacher.GetAttribute("innerText"); ;

                    Console.WriteLine("Alınan veri: " + data);


                    

                    // Belirli bir öğeyi beklemek için
                    bool isElementPresent = wait.Until(drv =>
                    {
                        try
                        {
                            return drv.FindElement(By.ClassName("OneByOne sapMGTHdrContent")) != null;
                        }
                        catch (NoSuchElementException)
                        {
                            return false; // Eleman mevcut değilse false döndür
                        }
                    });

                    if (isElementPresent)
                    {
                        var token = _tokenService.GenerateToken(request, "Test");
                        if (token == null)
                        {
                            throw new Exception("Token generation failed.");
                        }

                        var refreshToken = _tokenService.GenerateRefreshToken();
                        if (refreshToken == null)
                        {
                            throw new Exception("Refresh token generation failed.");
                        }

                        var authResult = new AuthResult
                        {
                            Success = true,
                            Token = token,
                            RefreshToken = refreshToken.Token
                        };

                        return Ok(authResult);
                    }
                    else
                    {
                        return StatusCode(400, new { message = "Login failed" });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login." });
            }
        }
    }
}
