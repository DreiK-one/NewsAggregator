using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleController> _logger;
        private readonly IArticleService _articleService;

        public ArticleController(IMapper mapper, 
            ILogger<ArticleController> logger, 
            IArticleService articleService)
        {
            _mapper = mapper;
            _logger = logger;
            _articleService = articleService;
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
                return BadRequest();
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
                    _logger.LogWarning($"{DateTime.Now}: Model is null in DeleteCategory method");
                    return BadRequest();
                }

                return RedirectToAction("GetArticlesOnAdminPanel", "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
