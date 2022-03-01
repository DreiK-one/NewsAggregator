using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        private readonly IRssService _rssService;
        private readonly IArticleService _articleService;

        public AdminController(ILogger<AdminController> logger,
            IRssService rssService,
            IArticleService articleService, 
            IMapper mapper)
        {
            _logger = logger;
            _rssService = rssService;
            _articleService = articleService;
            _mapper = mapper;
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
                return BadRequest();
            }
        }

        public async Task<IActionResult> GetNewsFromSources()
        {
            try
            {
                await _rssService.GetNewsFromSources();

                return RedirectToAction("GetArticlesOnAdminPanel", "Article");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        public async Task<IActionResult> GetArticlesOnAdminPanel()
        {
            try
            {
                var model = (await _articleService.GetAllNewsAsync())
                .Select(article => _mapper.Map<AllNewsOnHomeScreenViewModel>(article))
                .ToList();

                return View("ArticlesOnAdminPanel", model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
