using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleController> _logger;
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly ICategoryService _categoryService;

        public ArticleController(IMapper mapper,
            ILogger<ArticleController> logger,
            IArticleService articleService, 
            ISourceService sourceService,
            ICategoryService categoryService)
        {
            _mapper = mapper;
            _logger = logger;
            _articleService = articleService;
            _sourceService = sourceService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> ReadArticle(Guid id)
        {
            try
            {
                var article = await _articleService.GetArticleWithAllNavigationProperties(id);

                if (article == null)
                {
                    _logger.LogWarning($"{DateTime.Now}: Model is null in ReadArticle method");
                    return BadRequest();
                }

                var model = _mapper.Map<ReadArticleViewModel>(article);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var sources = (await _sourceService.GetAllSourcesAsync())
                    .Select(source => _mapper.Map<SourceModel>(source))
                    .ToList();

                var categories = (await _categoryService.GetAllCategoriesAsync())
                .Select(category => _mapper.Map<CategoryModel>(category))
                .ToList();

                //ViewBag.Sources = sources; // for tag-helpers in view, second option - MAY BE BAD PRACTICE???

                //ViewBag.Categories = categories; // for tag-helpers in view, second option - MAY BE BAD PRACTICE???

                var model = new CreateOrEditArticleViewModel()
                {
                    Sources = sources.Select(source => new SelectListItem(source.Name, source.Id.ToString())),
                    Categories = categories.Select(category => new SelectListItem(category.Name, category.Id.ToString()))
                };
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
        public async Task<IActionResult> CreateArticle(CreateOrEditArticleViewModel model)
        {
            try
            {
                if (model != null)
                {
                    await _articleService.CreateAsync(_mapper.Map<CreateOrEditArticleDto>(model));
                }
                return RedirectToAction("GetArticlesOnAdminPanel", "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(CreateOrEditArticleDto articleDto)
        {
            try
            {
                var sources = (await _sourceService.GetAllSourcesAsync())
                    .Select(source => _mapper.Map<SourceModel>(source))
                    .ToList();

                var categories = (await _categoryService.GetAllCategoriesAsync())
                .Select(category => _mapper.Map<CategoryModel>(category))
                .ToList();

                var model = _mapper.Map<CreateOrEditArticleViewModel>(articleDto);
                model.Categories = categories.Select(category => new SelectListItem(category.Name, category.Id.ToString()));
                model.Sources = sources.Select(source => new SelectListItem(source.Name, source.Id.ToString()));

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
        public async Task<IActionResult> EditArticle(CreateOrEditArticleViewModel model)
        {
            try
            {
                if (model != null)
                {
                    await _articleService.UpdateAsync(_mapper.Map<CreateOrEditArticleDto>(model));
                }
                return RedirectToAction("GetArticlesOnAdminPanel", "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var model = new DeleteArticleViewModel() { Id = id };
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
        public async Task<IActionResult> DeleteArticle(DeleteArticleViewModel model)
        {
            try
            {
                var delete = await _articleService.DeleteAsync(model.Id);

                if (delete == null)
                {
                    _logger.LogWarning($"{DateTime.Now}: Model is null in DeleteArticle method");
                    return BadRequest();
                }

                return RedirectToAction("GetArticlesOnAdminPanel", "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
