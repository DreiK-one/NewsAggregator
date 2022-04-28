using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.WebAPI.Models.Requests;
using NewsAggregator.WebAPI.Models.Responses;

namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TokenController> _logger;
        private readonly ITokenService _tokenService;
        
        public TokenController(IMapper mapper,
            ILogger<TokenController> logger,
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request)
        {
            try
            {
                var dto = _mapper.Map<LoginDto>(request);
                var response = await _tokenService.GetToken(dto, GetIpAddress());

                if (response == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                SetTokenCookie(response.RefreshToken);

                return Ok(_mapper.Map<AuthenticateResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refresh-token"];
                var response = await _tokenService.RefreshToken(refreshToken, GetIpAddress());

                if (response == null)
                {
                    return BadRequest(new { message = "Invalid token" });
                }

                SetTokenCookie(response.RefreshToken);

                return Ok(_mapper.Map<AuthenticateResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest request)
        {
            try
            {
                var token = request.Token ?? Request.Cookies["refresh-token"];

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "Token is required" });
                }

                var response = _tokenService.RevokeToken(token, GetIpAddress());
                return Ok(new { message = "Token is revoked" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        private void SetTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(2)
            };
            Response.Cookies.Append("refresh-token", refreshToken, cookieOptions);
        }

        private string GetIpAddress()
        {
            return Request.Headers.ContainsKey("X-Forwarded-For")
                ? Request.Headers["X-Forwarded-For"]
                : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }
    }
}
