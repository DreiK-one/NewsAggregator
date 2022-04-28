using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface ITokenService
    {
        Task RevokeToken(string token, string ipAddress);
        Task<JwtAuthDto> RefreshToken(string? refreshToken, string ipAddress);
        Task<JwtAuthDto> GetToken(LoginDto request, string getIpAddress);
    }
}
