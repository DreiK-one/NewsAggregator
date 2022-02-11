using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IMapper mapper, IArticleService articleService, ILogger<ArticleController> logger)
        {
            _mapper = mapper;
            _articleService = articleService;
            _logger = logger;
        }

        public async Task<IActionResult> ReadArticle(Guid id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: ReadArticle was called");

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
    }
}
