using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
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
        private readonly ICommentServiceCQS _commentServiceCQS;

        public CommentsController(ICommentServiceCQS commentServiceCQS,
            ILogger<ArticlesController> logger, 
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _commentServiceCQS = commentServiceCQS;
        }

        [HttpPost, Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(CreateCommentRequest request)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Request is null or invalid" });
                }

                await _commentServiceCQS.CreateAsync(_mapper.Map<CreateOrEditCommentDto>(request));
                return Ok(new ResponseMessage { Message = "Comment created!"});
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest(new ResponseMessage { Message = ex.Message});
            }
        }
    }
}
