using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces;


namespace NewsAggregator.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly IConfiguration _configuration;

        public HomeController(IMapper mapper,
            ILogger<HomeController> logger,
            IArticleService articleService,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _articleService = articleService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                if (page < 1)
                {
                    page = 1;
                }

                var pageSize = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

                var articles = (await _articleService.GetNewsByRatingByPageAsync(page - 1))
                    .Select(article => _mapper.Map<AllNewsOnHomeScreenViewModel>(article))
                    .OrderByDescending(article => article.CreationDate)
                    .ToList();

                var articlesCount = Convert.ToInt32(Math.Ceiling((double)
                    (await _articleService.GetAllNewsByRatingAsync()).Count() / pageSize));

                var pager = new Pager(articlesCount, page, pageSize);

                var model = new ArticlesByPagesViewModel()
                {
                    NewsList = articles,
                    Pager = pager
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}