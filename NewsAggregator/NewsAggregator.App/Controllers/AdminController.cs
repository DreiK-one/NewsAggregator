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
                var pageSize = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

                var pageAmount = Convert.ToInt32(Math.Ceiling((double)
                    (await _articleService.GetAllNewsAsync()).Count() / pageSize));

                var articles = (await _articleService.GetNewsByPageAsync(page - 1))
                .Select(article => _mapper.Map<AllNewsOnHomeScreenViewModel>(article))
                .OrderByDescending(article => article.CreationDate)
                .ToList();

                var model = new ArticlesByPagesViewModel()
                {
                    NewsList = articles,
                    PageAmount = pageAmount
                };

                return View("ArticlesOnAdminPanel", model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
