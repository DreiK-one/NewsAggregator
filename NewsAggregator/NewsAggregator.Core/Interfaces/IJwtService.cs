using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(UserDto user);
        public Task<Guid?> ValidateJwtToken(string token);
        public RefreshTokenDto GenerateRefreshToken(string ipAdress);
    }
}
