using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Domain.Services;

namespace NewsAggregator.App.Controllers
{
    public class RoleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public RoleController(IMapper mapper, IRoleService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var model = (await _roleService.GetAllRolesAsync())
                .Select(role => _mapper.Map<RoleViewModel>(role))
                .ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new RoleViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (model != null)
            {
                await _roleService.CreateAsync(_mapper.Map<RoleDto>(model));
            }
            return RedirectToAction("Index", "Role");
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var model = new DeleteRoleViewModel() { Id = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(DeleteRoleViewModel model)
        {
            var delete = await _roleService.DeleteAsync(model.Id);

            if (delete == null)
                return BadRequest();

            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var model = new RoleViewModel() { Id = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(RoleViewModel model)
        {
            if (model != null)
            {
                await _roleService.UpdateAsync(_mapper.Map<RoleDto>(model));
            }
            return RedirectToAction("Index", "Role");
        }
    }
}
