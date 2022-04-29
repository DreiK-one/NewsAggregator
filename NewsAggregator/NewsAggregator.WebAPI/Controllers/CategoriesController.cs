using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.WebAPI.Models.Requests;
using NewsAggregator.WebAPI.Models.Responses;
using System.Net;

namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService,
            ILogger<ArticlesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(CategoryWithArticlesDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), 500)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                if (name == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Name is null" });
                }

                var category = await _categoryService.GetCategoryByNameWithArticlesAsync(name);
                if (category != null)
                {
                    return Ok(category);
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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), 500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                if (categories != null)
                {
                    return Ok(categories);
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

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(CategoryWithArticlesDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), 500)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new ResponseMessage { Message = "Identificator is null" });
                }

                var categories = await _categoryService.GetCategoryByIdWithArticlesAsync(id);
                if (categories != null)
                {
                    return Ok(categories);
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
