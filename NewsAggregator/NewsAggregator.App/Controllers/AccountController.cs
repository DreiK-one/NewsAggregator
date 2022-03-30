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
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginModel model)
        {
            if (await _accountService.CheckPassword(model.Email, model.Password))
            {
                var claims = new List<Claim>() { new Claim(ClaimTypes.Name, model.Email)};
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _accountService.CheckUserWithThatEmailIsExistAsync(model.Email))
                {
                    var userId = await _accountService.CreateUserAsync(model.Email);
                    await _accountService.SetRoleAsync(userId, "User");
                    await _accountService.SetPasswordAsync(userId, model.Password);
                }
                else
                {
                    ModelState.TryAddModelError("UserAlreadyExists", "User is already exist!");
                }
            }
            return View(model);
        }
    }
}
