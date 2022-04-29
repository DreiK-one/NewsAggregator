using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

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
                if (await _accountService.CheckPasswordByEmailAsync(model.Email, model.Password))
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
        [Authorize]
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
        [Authorize]
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
        [ApiExplorerSettings(IgnoreApi = true)]
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Manage()
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangeEmail()
        {
            try
            {
                var nickname = HttpContext.User.Claims.FirstOrDefault().Value;
                var model = new ChangeEmailViewModel
                {
                    UserId = await _accountService.GetUserIdByNicknameAsync(nickname)
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeEmailConfirm(ChangeEmailViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        await _accountService.UpdateEmail(model.UserId, model.NewEmail);
                        return Redirect(nameof(ChangesApplied));
                    }
                }

                return View(nameof(ChangeEmail), model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            try
            {
                var nickname = HttpContext.User.Claims.FirstOrDefault().Value;
                var model = new ChangePasswordViewModel
                {
                    UserId = await _accountService.GetUserIdByNicknameAsync(nickname)
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePasswordConfirm(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _accountService.CheckPasswordByIdAsync(model.UserId, model.CurrentPassword))
                    {
                        await _accountService.SetPasswordAsync(model.UserId, model.NewPassword);
                        return Redirect(nameof(ChangesApplied));
                    }
                    return View("IncorrectCurrentPassword", model);
                }

                return View(nameof(ChangePassword), model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return RedirectToAction("Error500");
            }
        } 

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangeNickname()
        {
            try
            {
                var nickname = HttpContext.User.Claims.FirstOrDefault().Value;
                var model = new ChangeNicknameViewModel
                {
                    UserId = await _accountService.GetUserIdByNicknameAsync(nickname)
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeNicknameConfirm(ChangeNicknameViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentName = await _accountService.GetUserNicknameByIdAsync(model.UserId);

                    if (currentName.ToUpperInvariant().Equals(model.CurrentNickname))
                    {
                        await _accountService.UpdateNickname(model.UserId, model.NewNickname);
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        return RedirectToAction(nameof(ChangesApplied));
                    }
                    return View("IncorrectCurrentNickname", model);
                }

                return View(nameof(ChangeNickname), model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [Authorize]
        public async Task<IActionResult> ChangesApplied()
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
