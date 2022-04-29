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
    }
}
