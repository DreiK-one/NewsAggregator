﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IRoleService _roleService;

        public UserController(IMapper mapper,
            ILogger<UserController> logger,
            IUserService userService, 
            IRoleService roleService)
        {
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _roleService = roleService;
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                var roles = (await _roleService.GetAllRolesAsync())
                    .Select(role => _mapper.Map<RoleModel>(role))
                    .ToList();

                var model = _mapper.Map<CreateOrEditUserViewModel>(user);
                model.Roles = roles.Select(role => new SelectListItem(role.Name, role.Id.ToString()));
                model.RoleId = await _roleService.GetRoleIdByUserIdAsync(id);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }      
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(CreateOrEditUserViewModel model)
        {
            try
            {
                if (model != null)
                {
                    await _roleService.ChangeUserRole(_mapper.Map<UserRoleDto>(model));

                    await _userService.UpdateAsync(_mapper.Map<CreateOrEditUserDto>(model));
                    return RedirectToAction("Index", "User");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
