using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService,
            ILogger<ArticlesController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (Guid.Empty == id)
                {
                    return BadRequest();
                }

                var article = await _articleService.GetArticleAsync(id);
                if (article != null)
                {
                    return Ok(article);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                 return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
