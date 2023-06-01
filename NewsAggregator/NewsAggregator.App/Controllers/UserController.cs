using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
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

                var model = _mapper.Map<EditUserViewModel>(user);
                model.Roles = roles.Select(role => new SelectListItem(role.Name, role.Id.ToString()));
                model.RoleId = await _roleService.GetRoleIdByUserIdAsync(id);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
            }      
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        await _roleService.ChangeUserRole(_mapper.Map<UserRoleDto>(model));

                        await _userService.UpdateAsync(_mapper.Map<CreateOrEditUserDto>(model));
                        return RedirectToAction("Index", "User");
                    }  
                }

                var roles = (await _roleService.GetAllRolesAsync())
                    .Select(role => _mapper.Map<RoleModel>(role))
                    .ToList();
                model.Roles = roles.Select(role => new SelectListItem(role.Name, role.Id.ToString()));

                return View(nameof(Edit), model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var article = await _userService.GetUserByIdAsync(id);
                var model = _mapper.Map<DeleteUserViewModel>(article);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(DeleteUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var delete = await _userService.DeleteAsync(model.Id);
                    if (delete == null)
                    {
                        _logger.LogWarning($"{DateTime.Now}: Model is null in DeleteUser method");
                        return BadRequest();
                    }
                    return RedirectToAction("Index", "User");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var roles = (await _roleService.GetAllRolesAsync())
                    .Select(role => _mapper.Map<RoleModel>(role))
                    .ToList();

                var model = new CreateUserViewModel
                {
                    Roles = roles.Select(role => new SelectListItem(role.Name, role.Id.ToString()))
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        await _userService.CreateAsync(_mapper.Map<CreateOrEditUserDto>(model));
                        return RedirectToAction("Index", "User");
                    }
                }
                var roles = (await _roleService.GetAllRolesAsync())
                    .Select(role => _mapper.Map<RoleModel>(role))
                    .ToList();

                model.Roles = roles.Select(role => new SelectListItem(role.Name, role.Id.ToString()));

                return View(nameof(Create), model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
