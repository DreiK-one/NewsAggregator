using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.WebAPI.Models.Requests;

namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService,
            ILogger<ArticlesController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        //[HttpPost]
        //public async Task<IActionResult> GetByName(CreateCommentRequest request)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest();
        //        }

        //        if (request != null)
        //        {
        //            var principal = HttpContext.User;
        //            if (principal != null)
        //            {
        //                foreach (var claim in principal.Claims)
        //                {
        //                    model.UserId = await _accountService.GetUserIdByNicknameAsync(claim.Value);
        //                    await _commentService.CreateAsync(_mapper.Map<CreateOrEditCommentDto>(model));

        //                    return Redirect($"~/Article/ReadArticle/{model.ArticleId}");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return NoContent();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
        //        throw;
        //    }
        //}
    }
}
