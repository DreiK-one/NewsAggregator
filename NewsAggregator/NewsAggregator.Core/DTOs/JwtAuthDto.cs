using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.DTOs
{
    public class JwtAuthDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public List<UserRoleDto> RoleNames { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }

        public JwtAuthDto(UserDto user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            Email = user.Email;
            RoleNames = user.UserRoles;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
