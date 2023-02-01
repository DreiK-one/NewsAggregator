using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.Core.Interfaces.WebApiInterfaces;
using NewsAggregator.WebAPI.Models.Requests;
using NewsAggregator.WebAPI.Models.Responses;
using System.Net;


namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenService _tokenService;
        private readonly IAccountServiceCQS _accountServiceCQS;

        public AccountController(IMapper mapper,
            ILogger<AccountController> logger,
            ITokenService tokenService,
            IAccountServiceCQS accountServiceCQS)
        {
            _tokenService = tokenService;
            _logger = logger;
            _mapper = mapper;
            _accountServiceCQS = accountServiceCQS;
        }

        [HttpPost("authenticate"), AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticateResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Request is null or invalid" });
                }

                var dto = _mapper.Map<LoginDto>(request);
                var response = await _tokenService.GetToken(dto, GetIpAddress());

                if (response == null)
                {
                    return BadRequest(new ResponseMessage{ Message = "Username or password is incorrect" });
                }

                SetTokenCookie(response.RefreshToken);

                return Ok(_mapper.Map<AuthenticateResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPost("register"), AllowAnonymous]
        [ProducesResponseType(typeof(RegisterResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Request is null or invalid" });
                }

                var user = _mapper.Map<RegisterDto>(request);
                var response = await _accountServiceCQS.CreateUserAsync(user);

                return Ok(new ResponseMessage { Message = "Registration success!"});
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPut("change-password"), Authorize] 
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Request is null or invalid" });
                }

                var response = await _accountServiceCQS
                    .ChangePasswordAsync(request.Email, request.CurrentPassword, request.NewPassword);

                return Ok(new ResponseMessage { Message = "Password successfully changed!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        
        [HttpPost("refresh-token"), AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticateResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refresh-token"];
                var response = await _tokenService.RefreshToken(refreshToken, GetIpAddress());

                if (response == null)
                {
                    return BadRequest(new ResponseMessage{ Message = "Invalid token" });
                }

                SetTokenCookie(response.RefreshToken);

                return Ok(_mapper.Map<AuthenticateResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        
        [HttpPost("revoke-token"), Authorize]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult RevokeToken(RevokeTokenRequest request)
        {
            try
            {
                var token = request.Token ?? Request.Cookies["refresh-token"];

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new ResponseMessage{ Message = "Token is required" });
                }

                var response = _tokenService.RevokeToken(token, GetIpAddress());
                return Ok(new { message = "Token is revoked" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        private void SetTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
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
