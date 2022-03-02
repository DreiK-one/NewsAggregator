using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, 
            ILogger<UserController> logger, 
            IUserService userService)
        {
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = (await _userService.GetAllUsersWithAllInfoAsync())
                .Select(user => _mapper.Map<UserViewModel>(user))
                .ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogError($"{DateTime.Now}: Model is null in Details method");
                    return BadRequest();
                }

                var model = _mapper.Map<UserViewModel>(user);
                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            try
            {
                var model = new UserViewModel() { Id = id };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            try
            {
                if (model != null)
                {
                    await _userService.UpdateAsync(_mapper.Map<UserDto>(model));
                }
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
