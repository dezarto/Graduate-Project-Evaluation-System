using GraduateProjectEvaluationSystemAPI.Application.DTOs;
using GraduateProjectEvaluationSystemAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace GraduateProjectEvaluationSystembAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginCatsController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IUserAppService _userAppService;

        public LoginCatsController(ITokenService tokenService, IUserAppService userAppService)
        {
            _tokenService = tokenService;
            _userAppService = userAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
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
                    driver.Navigate().GoToUrl("https://cats.iku.edu.tr/access/login");

                    var usernameInput = driver.FindElement(By.Id("eid"));
                    var passwordInput = driver.FindElement(By.Id("pw"));

                    usernameInput.SendKeys(request.Username);
                    passwordInput.SendKeys(request.Password);

                    var loginButton = driver.FindElement(By.Id("submit"));
                    loginButton.Click();

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                    bool isElementPresent = wait.Until(drv =>
                    {
                        try
                        {
                            return drv.FindElement(By.ClassName("Mrphs-userNav__submenuitem--profilepicture")) != null;
                        }
                        catch (NoSuchElementException)
                        {
                            return false;
                        }
                    });

                    if (isElementPresent)
                    {
                        var profileButton = driver.FindElement(By.ClassName("Mrphs-userNav__submenuitem--profilepicture"));
                        profileButton.Click();

                        var fullNameElement = driver.FindElement(By.ClassName("Mrphs-userNav__submenuitem--fullname"));
                        string fullName = fullNameElement.Text;

                        var token = _tokenService.GenerateToken(request);
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

                        var userExists = await _userAppService.ExistsByStudentNumberAppAsync(request.Username);

                        if (!userExists) 
                        {
                            var newUser = new UserDTO
                            {
                                ProfessorId = 1, // Default Professor
                                StudentNumber = request.Username,
                                Role = "Student",
                                Email = request.Username + "@stu.iku.edu.tr",
                                FullName = fullName,
                            };

                            await _userAppService.AddUserAppAsync(newUser);
                        }

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
