using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Domain.Services;

namespace NewsAggregator.App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;

        public CategoryController(IMapper mapper, CategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var model = (await _categoryService.GetAllCategoriesAsync())
                .Select(category => _mapper.Map<CategoryViewModel>(category))
                .ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CategoryViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            if (model != null)
            {
                await _categoryService.CreateAsync(_mapper.Map<CategoryDto>(model));
            }
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public IActionResult Delete()
        {
            var model = new CategoryViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(CategoryViewModel model)
        {
            if (model != null)
            {
                await _categoryService.CreateAsync(_mapper.Map<CategoryDto>(model));
            }
            return RedirectToAction("Index", "Category");
        }
    }
}
