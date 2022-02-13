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
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IMapper mapper, IUserService userService, ILogger<UserController> logger)
        {
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: Index was called");

                var model = (await _userService.GetAllUsersWithAllInfoAsync())
                .Select(user => _mapper.Map<UserViewModel>(user))
                .ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: Details was called");

                var user = _userService.GetUserByIdAsync(id);

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
                return BadRequest();
            }

            

        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: Delete was called");

                var model = new UserViewModel() { Id = id };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: EditUser was called");

                if (model != null)
                {
                    await _userService.UpdateAsync(_mapper.Map<UserDto>(model));
                }
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
