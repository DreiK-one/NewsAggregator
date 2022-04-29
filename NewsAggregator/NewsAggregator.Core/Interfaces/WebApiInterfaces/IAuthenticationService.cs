using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces.WebApiInterfaces
{
    public interface IAuthenticationService
    {
        Task<UserDto> CreateUserByApiAsync(RegisterDto dto);
        Task<int?> ChangePasswordByApiAsync(string email, string currentPass, string newPass);
    }
}
