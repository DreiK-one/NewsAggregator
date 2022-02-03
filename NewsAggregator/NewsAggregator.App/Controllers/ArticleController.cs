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

        public ArticleController(IMapper mapper, IArticleService articleService)
        {
            _mapper = mapper;
            _articleService = articleService;
        }

        public async Task<IActionResult> ReadArticle(Guid id)
        {
            var article = await _articleService.GetArticleWithAllNavigationProperties(id);

            if (article == null)
                return BadRequest();

            var model = _mapper.Map<ReadArticleViewModel>(article);

            return View(model);
        }
    }
}
