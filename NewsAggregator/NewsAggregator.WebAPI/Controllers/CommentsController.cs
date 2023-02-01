using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
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
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create(CreateCommentRequest request)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Request is null or invalid" });
                }

                await _commentServiceCQS.CreateAsync(_mapper.Map<CreateOrEditCommentDto>(request));
                return Ok(new ResponseMessage { Message = "Comment created!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin, Moderator")]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Edit(Guid id, [FromBody]EditCommentRequest request)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Request is null or invalid" });
                }
                var comment = await _commentServiceCQS.GetByIdAsync(id);
                if (comment == null)
                {
                    return BadRequest(new ResponseMessage { Message = $"Comment with id {id} not found" });
                }

                comment.Text = request.Text;

                await _commentServiceCQS.EditAsync(comment);
                return Ok(new ResponseMessage { Message = "Comment changed!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseMessage), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new ResponseMessage { Message = "Identificator is null" });
                }

                await _commentServiceCQS.DeleteAsync(id);
                return Ok(new ResponseMessage { Message = "Comment deleted!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new ResponseMessage { Message = ex.Message });
            }
        }
    }
}
