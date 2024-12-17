using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace GEPS.Filter
{
    public class RoleFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Token'ı al
            var token = context.HttpContext.Session.GetString("BearerToken");

            if (!string.IsNullOrEmpty(token))
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jsonToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

                // Token'dan role bilgisini al
                var userRole = jsonToken?.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                // Eğer role bilgisi varsa, context'e kaydet
                if (!string.IsNullOrEmpty(userRole))
                {
                    context.HttpContext.Items["UserRole"] = userRole;
                }
                else
                {
                    // Eğer token'dan rol alınamadıysa, varsayılan bir rol atayabilirsiniz
                    context.HttpContext.Items["UserRole"] = "Student"; // Örneğin varsayılan olarak "Student"
                }
            }
            else
            {
                // Eğer token yoksa, varsayılan olarak bir rol atayabilirsiniz
                context.HttpContext.Items["UserRole"] = "Student"; // Varsayılan rol
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // İhtiyaç duyulursa işlem yapılabilir
        }
    }
}
