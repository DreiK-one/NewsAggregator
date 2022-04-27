using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Domain.Services;

namespace NewsAggregator.App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;

        public RoleController(IMapper mapper, 
            ILogger<RoleController> logger, 
            IRoleService roleService)
        {
            _mapper = mapper;
            _logger = logger;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = (await _roleService.GetAllRolesAsync())
                .Select(role => _mapper.Map<RoleModel>(role))
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
        public IActionResult Create()
        {
            try
            {
                var model = new RoleModel();
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
        public async Task<IActionResult> CreateRole(RoleModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        await _roleService.CreateAsync(_mapper.Map<RoleDto>(model));
                    }
                    return RedirectToAction("Index", "Role");
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var role = await _roleService.GetRoleAsync(id);
                var model = _mapper.Map<DeleteRoleViewModel>(role);
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
        public async Task<IActionResult> DeleteRole(DeleteRoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var delete = await _roleService.DeleteAsync(model.Id);
                    if (delete == null)
                    {
                        _logger.LogWarning($"{DateTime.Now}: Model is null in DeleteRole method");
                        return BadRequest();
                    }
                    return RedirectToAction("Index", "Role");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var role = await _roleService.GetRoleAsync(id);
                var model = _mapper.Map<RoleModel>(role);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(RoleModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        await _roleService.UpdateAsync(_mapper.Map<RoleDto>(model));
                    }
                    return RedirectToAction("Index", "Role");
                }

                return View(model);      
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
