using AutoMapper;
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
    public class CommentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticlesController> _logger;
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService,
            ILogger<ArticlesController> logger, 
            IMapper mapper)
        {
            _commentService = commentService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(CreateCommentRequest request)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest(new ErrorModel { Message = "Request is null or invalid" });
                }

                await _commentService.CreateAsync(_mapper.Map<CreateOrEditCommentDto>(request));
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest(new ErrorModel { Message = ex.Message});
            }
        }
    }
}
