﻿using AutoMapper;
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

        [HttpGet]
        public async Task<IActionResult> ShowCategoryWithNews(string name)
        {
            try
            {
                var category = await _categoryService.GetCategoryByNameWithArticlesAsync(name);
                var model = _mapper.Map<CategoryWithArticlesViewModel>(category);

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
