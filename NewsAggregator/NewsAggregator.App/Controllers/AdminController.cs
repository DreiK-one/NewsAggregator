using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        private readonly IRssService _rssService;
        private readonly IArticleService _articleService;
        private readonly IConfiguration _configuration;

        public AdminController(ILogger<AdminController> logger,
            IRssService rssService,
            IArticleService articleService,
            IMapper mapper, 
            IConfiguration configuration)
        {
            _logger = logger;
            _rssService = rssService;
            _articleService = articleService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        public async Task<IActionResult> GetNewsFromSources()
        {
            try
            {
                await _rssService.GetNewsFromSourcesAsync();

                return RedirectToAction("GetArticlesOnAdminPanel", "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        public async Task<IActionResult> GetArticlesOnAdminPanel(int page = 1)
        {
            try
            {
                if (page < 1)
                {
                    page = 1;
                }

                var pageSize = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

                var articles = (await _articleService.GetNewsByPageAsync(page - 1))
                .Select(article => _mapper.Map<AllNewsOnHomeScreenViewModel>(article))
                .OrderByDescending(article => article.CreationDate)
                .ToList();

                var articlesCount = Convert.ToInt32(Math.Ceiling((double)
                    (await _articleService.GetAllNewsAsync()).Count() / pageSize));

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
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [Route("/hangfire")]
        public async Task<IActionResult> Hangfire()
        {
            try
            {
                return View("/hangfire");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
