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
    }
}
