using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Core.Interfaces.WebApiInterfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.WebApiServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<JwtService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;

        public AuthenticationService(IMapper mapper, 
            ILogger<JwtService> logger,
            IAccountService accountService)
        {
            _mapper = mapper;
            _logger = logger;
            _accountService = accountService;
        }
        public async Task<UserDto> CreateUserByApiAsync(RegisterDto dto)
        {
            if (_accountService.ValidateIsEmailExists(dto.Email))
            {
                throw new ArgumentException($"Email {dto.Email} is already exists");
            }

            if (_accountService.ValidateIsNicknameExists(dto.Nickname))
            {
                throw new ArgumentException($"Nickname {dto.Nickname} is already exists");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("Password is required");
            }

            var userId = await _accountService.CreateUserAsync(dto.Email, dto.Nickname);
            await _accountService.SetRoleAsync(userId, "User");
            await _accountService.SetPasswordAsync(userId, dto.Password);

            return await _accountService.GetUserByEmailAsync(dto.Email);
        }
    }
}
