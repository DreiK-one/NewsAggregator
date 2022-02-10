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

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var model = (await _userService.GetAllUsersWithAllInfoAsync())
                .Select(user => _mapper.Map<UserViewModel>(user))
                .ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var model = new UserViewModel() { Id = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (model != null)
            {
                await _userService.UpdateAsync(_mapper.Map<UserDto>(model));
            }
            return RedirectToAction("Index", "User");
        }
    }
}
