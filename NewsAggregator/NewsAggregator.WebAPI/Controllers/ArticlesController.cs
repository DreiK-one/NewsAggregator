using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.WebAPI.Models.Responses;
using System.Net;

namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticlesController> _logger;
        private readonly IArticleServiceCQS _articleServiceCQS;

        public ArticlesController(IArticleServiceCQS articleServiceCQS,
            ILogger<ArticlesController> logger,
            IMapper mapper)
        {
            _articleServiceCQS = articleServiceCQS;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}"), AllowAnonymous]
        [ProducesResponseType(typeof(ArticleDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), 500)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new ResponseMessage { Message = "Identificator is null"});
                }

                var article = await _articleServiceCQS.GetArticleById(id);
                if (article.Coefficient <= 0 && !User.IsInRole("Admin") )
                {
                    return Forbid();
                }

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
                return StatusCode(500, new ResponseMessage{ Message = ex.Message });
            }
        }

        [HttpGet, AllowAnonymous]
        [ProducesResponseType(typeof(ArticleDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), 500)]
        public async Task<IActionResult> Get(int? page)
        {
            try
            {
                var articles = await _articleServiceCQS.GetAllArticles();
                if (!User.IsInRole("Admin"))
                {
                    articles = articles.Where(art => art.Coefficient > 0).ToList();
                }

                if (page >= 0 && page != null)
                {
                    if (!User.IsInRole("Admin"))
                    {
                        articles = await _articleServiceCQS
                         .GetPositiveArticlesByPage(Convert.ToInt32(page));
                    }
                    else
                    {
                        articles = await _articleServiceCQS
                          .GetArticlesByPage(Convert.ToInt32(page));
                    }
                }

                if (articles != null)
                {
                    return Ok(articles);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new ResponseMessage { Message = ex.Message });
            }
        }
    }
}
