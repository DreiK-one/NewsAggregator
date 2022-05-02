using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.WebApiInterfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(UserDto user);
        public Task<Guid?> ValidateJwtToken(string token);
        public RefreshTokenDto GenerateRefreshToken(string ipAdress);
    }
}
