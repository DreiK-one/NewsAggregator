using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IRssService _rssService;

        public AdminController(ILogger<AdminController> logger,
            IRssService rssService)
        {
            _logger = logger;
            _rssService = rssService;
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

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
