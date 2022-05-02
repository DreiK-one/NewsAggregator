using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.WebApiInterfaces
{
    public interface ITokenService
    {
        Task RevokeToken(string token, string ipAddress);
        Task<JwtAuthDto> RefreshToken(string? refreshToken, string ipAddress);
        Task<JwtAuthDto> GetToken(LoginDto request, string getIpAddress);
    }
}
