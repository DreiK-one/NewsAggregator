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
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IMapper mapper, ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _mapper = mapper;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: Index was called");

                var model = (await _categoryService.GetAllCategoriesAsync())
                .Select(category => _mapper.Map<CategoryViewModel>(category))
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
        public IActionResult Create()
        {
            try
            {
               _logger.LogInformation($"{DateTime.Now}: Create was called");

                var model = new CategoryViewModel();
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
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: CreateCategory was called");

                if (model != null)
                {
                    await _categoryService.CreateAsync(_mapper.Map<CategoryDto>(model));
                }
                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
            
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: Delete was called");

                var model = new DeleteCategoryViewModel() { Id = id };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryViewModel model)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: DeleteCategory was called");

                var delete = await _categoryService.DeleteAsync(model.Id);

                if (delete == null)
                {
                    _logger.LogWarning($"{DateTime.Now}: Model is null in DeleteCategory method");
                    return BadRequest();
                }

                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: Edit was called");

                var model = new CategoryViewModel() { Id = id };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(CategoryViewModel model)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: EditCategory was called");

                if (model != null)
                {
                    await _categoryService.UpdateAsync(_mapper.Map<CategoryDto>(model));
                }
                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            }
            
        }
    }
}
