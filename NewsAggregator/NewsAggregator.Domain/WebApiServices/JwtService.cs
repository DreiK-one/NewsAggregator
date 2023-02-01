using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.WebApiInterfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace NewsAggregator.Domain.WebApiServices
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;

        public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateJwtToken(UserDto user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Encoding.UTF8.GetBytes(_configuration["AppSettings:Secret"]);

                var claims = user.UserRoles
                    .Select(rn => new Claim(ClaimTypes.Role, rn.Role.Name))
                    .ToList();
                claims.Add(new Claim("id", user.Id.ToString("D")));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                        SecurityAlgorithms.HmacSha256),
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid?> ValidateJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.UTF8.GetBytes(_configuration["AppSettings:Secret"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
                return userId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return null;
            }
        }

        public RefreshTokenDto GenerateRefreshToken(string ipAddress)
        {
            try
            {
                var refreshToken = new RefreshTokenDto()
                {
                    Token = Guid.NewGuid().ToString("D"),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
                return refreshToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            } 
        }
    }
}
