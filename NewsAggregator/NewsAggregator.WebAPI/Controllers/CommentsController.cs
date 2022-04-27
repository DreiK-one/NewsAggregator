using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.WebAPI.Models.Requests;

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
        public async Task<IActionResult> Create(CreateCommentRequest request)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest();
                }

                await _commentService.CreateAsync(_mapper.Map<CreateOrEditCommentDto>(request));
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
