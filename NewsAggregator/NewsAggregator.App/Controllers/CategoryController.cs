using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Domain.Services;

namespace NewsAggregator.App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(IMapper mapper, 
            ILogger<CategoryController> logger, 
            ICategoryService categoryService)
        {
            _mapper = mapper;
            _logger = logger;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = (await _categoryService.GetAllCategoriesAsync())
                .Select(category => _mapper.Map<CategoryModel>(category))
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
                var model = new CategoryModel();
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
        public async Task<IActionResult> CreateCategory(CategoryModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        await _categoryService.CreateAsync(_mapper.Map<CategoryDto>(model));
                    }
                    return RedirectToAction("Index", "Category");
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
                var category = await _categoryService.GetCategoryAsync(id);
                var model = _mapper.Map<DeleteCategoryViewModel>(category);
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
        public async Task<IActionResult> DeleteCategory(DeleteCategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var delete = await _categoryService.DeleteAsync(model.Id);
                    if (delete == null)
                    {
                        _logger.LogWarning($"{DateTime.Now}: Model is null in DeleteCategory method");
                        return BadRequest();
                    }
                    return RedirectToAction("Index", "Category");
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
                var category = await _categoryService.GetCategoryAsync(id);
                var model = _mapper.Map<CategoryModel>(category);
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
        public async Task<IActionResult> EditCategory(CategoryModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        await _categoryService.UpdateAsync(_mapper.Map<CategoryDto>(model));
                    }
                    return RedirectToAction("Index", "Category");
                }

                return View(model);      
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        public async Task<IActionResult> ShowCategoryWithNews(string name)
        {
            try
            {
                var category = await _categoryService.GetCategoryByNameWithArticlesAsync(name);
                if (category == null)
                {
                    return RedirectToAction("Error404", "Error");
                }
                var model = _mapper.Map<CategoryWithArticlesViewModel>(category);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            } 
        }
    }
}
