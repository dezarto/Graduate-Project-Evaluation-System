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

            // Eğer token yoksa, frontend'in gönderdiğinden emin olmalısınız
            if (string.IsNullOrEmpty(token))
            {
                token = context.Session.GetString("BearerToken"); // Eğer frontend'den gelmediyse, session'dan alınabilir
            }

            if (!string.IsNullOrEmpty(token))
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jsonToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

                // Token'ın içinden rolü çıkartıyoruz
                var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                if (!string.IsNullOrEmpty(role))
                {
                    context.Items["UserRole"] = role; // Rol bilgisini HttpContext.Items'e kaydediyoruz
                }
            }

            await _next(context);
        }
    }
}
