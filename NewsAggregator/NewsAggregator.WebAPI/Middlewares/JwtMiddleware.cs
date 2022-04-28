using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.WebAPI.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next,
            IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IJwtService jwtService, IAccountService accountService)
        {
            var token = context.Request.Headers["Authrorization"]
                .FirstOrDefault()?
                .Split(" ")
                .Last();

            var userId = await jwtService.ValidateJwtToken(token);

            if (userId != null)
            {
                context.Items["User"] = accountService.GetUserById(userId.Value);
            }

            await _next(context);
        }
    }
}
