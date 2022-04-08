using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NewsAggregator.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(IMapper mapper,
            ILogger<AccountController> logger,
            IAccountService accountService)
        {
            _mapper = mapper;
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    var model = new AccountLoginModel
                    {
                        ReturnUrl = returnUrl
                    };
                    return View(model);
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginModel model)
        {
            try
            {
                if (await _accountService.CheckPassword(model.Email, model.Password))
                {
                    var userId = (await _accountService.GetUserIdByEmailAsync(model.Email))
                        .GetValueOrDefault();

                    var userNickname = await _accountService.GetUserNicknameByIdAsync(userId);

                    var roleClaims = (await _accountService.GetRolesAsync(userId))
                        .Select(roleName => new Claim(ClaimTypes.Role, roleName));

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, userNickname)
                    };
                    claims.AddRange(roleClaims);

                    var claimsIdentity = new ClaimsIdentity(claims, authenticationType: "Cookies");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return Redirect(model.ReturnUrl ?? "/");
                }

                return View("IncorrectPassword", model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            } 
        }

        [HttpGet]
        public async Task<IActionResult> Register(string? returnUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    var model = new AccountRegisterModel
                    {
                        ReturnUrl = returnUrl
                    };
                    return View(model);
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!await _accountService.CheckUserWithThatEmailIsExistAsync(model.Email))
                    {
                        var userId = await _accountService.CreateUserAsync(model.Email, model.Nickname);
                        await _accountService.SetRoleAsync(userId, "User");
                        await _accountService.SetPasswordAsync(userId, model.Password);

                        var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, model.Nickname),
                        new Claim(ClaimTypes.Role, "User")
                    };
                        var claimsIdentity = new ClaimsIdentity(claims, authenticationType: "Cookies");

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                        return Redirect(model.ReturnUrl ?? "/");
                    }
                    else
                    {
                        ModelState.TryAddModelError("UserAlreadyExists", "User is already exist!");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            } 
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }  
        }

        [HttpPost]
        public async Task<IActionResult> LogoutConfirm()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("access-denied")]
        public async Task<IActionResult> AccessDenied()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
