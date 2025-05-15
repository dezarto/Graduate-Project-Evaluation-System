using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GEPS.Filter
{
    public class RoleFilterAttribute : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public RoleFilterAttribute(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Session.GetString("BearerToken");
            var user = context.HttpContext.User; // Düzeltilmiş: User doğrudan HttpContext'ten alınıyor

            if (!string.IsNullOrEmpty(token))
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jsonToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

                var username = jsonToken?.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
                var userRole = jsonToken?.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                if (!string.IsNullOrEmpty(username))
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "Bearer");
                    context.HttpContext.User = new ClaimsPrincipal(identity);
                }

                if (string.IsNullOrEmpty(userRole) || userRole != _requiredRole)
                {
                    if (userRole == "Student" && userRole != _requiredRole)
                    {
                        context.Result = new RedirectResult("/Student/TeamHome");
                        return;
                    }
                    else if (userRole == "Professor" && userRole != _requiredRole)
                    {
                        context.Result = new RedirectResult("/Professor/TeamHomeProfessor");
                        return;
                    }
                    else if (userRole == "Admin" && userRole != _requiredRole)
                    {
                        context.Result = new RedirectResult("/Admin/GetAllProfessor");
                        return;
                    }
                    else
                    {
                        context.Result = new ContentResult
                        {
                            StatusCode = 403,
                            Content = "Unauthorized access."
                        };
                    }
                }
            }
            else
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "Token not found. Token error!! Contact the authorized person"
                };
            }
        }
    }
}
