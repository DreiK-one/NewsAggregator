using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.WebAPI.Models.Responses;
using System.Net;


namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryServiceCQS _categoryServiceCQS;

        public CategoriesController(ILogger<CategoriesController> logger, 
            ICategoryServiceCQS categoryServiceCQS)
        {
            _logger = logger;
            _categoryServiceCQS = categoryServiceCQS;
        }

        [HttpGet("{id}")]
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

                var category = await _categoryServiceCQS
                    .GetCategoryByIdWithArticlesAsync(id);

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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryWithArticlesDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(string? name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var category = await _categoryServiceCQS
                        .GetCategoryByNameWithArticlesAsync(Convert.ToString(name));

                    if (category != null)
                    {
                        return Ok(category);
                    }
                }
                else
                {
                    var categories = await _categoryServiceCQS.GetAllCategories();
                    if (categories != null)
                    {
                        return Ok(categories);
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new ResponseMessage { Message = ex.Message });
            }
        }   
    }
}
