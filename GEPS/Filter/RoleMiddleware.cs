using System.IdentityModel.Tokens.Jwt;

namespace GEPS.Filter
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                token = context.Session.GetString("BearerToken");
            }

            if (!string.IsNullOrEmpty(token))
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jsonToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

                var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                if (!string.IsNullOrEmpty(role))
                {
                    context.Items["UserRole"] = role;
                }
            }

            await _next(context);
        }
    }
}
