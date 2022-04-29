using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.WebAPI.Filters;
using NewsAggregator.WebAPI.Models.Requests;
using NewsAggregator.WebAPI.Models.Responses;
using NewsAggregator.WebAPI.Tools.Specs;
using System.Net;

namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticlesController> _logger;
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService,
            ILogger<ArticlesController> logger,
            IMapper mapper)
        {
            _articleService = articleService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("for-user/{id}")]
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

                var article = await _articleService.GetArticleWithAllNavigationPropertiesByRating(id);
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

        [HttpGet("for-user")]
        [ProducesResponseType(typeof(ArticleDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), 500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var article = await _articleService.GetAllNewsByRatingAsync();
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
                return StatusCode(500, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpGet("for-admin"), Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ArticleDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), 500)]
        public async Task<IActionResult> GetAsAdmin()
        {
            try
            {
                var article = await _articleService.GetAllNewsAsync();
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
                return StatusCode(500, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpGet("for-admin/{id}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ArticleDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), 500)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByIdAsAdmin(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new ResponseMessage { Message = "Identificator is null" });
                }

                var article = await _articleService.GetArticleWithAllNavigationProperties(id);
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
                return StatusCode(500, new ResponseMessage { Message = ex.Message });
            }
        }
    }
}
