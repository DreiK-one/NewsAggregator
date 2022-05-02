using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;


namespace NewsAggregator.App.Controllers
{
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CommentController> _logger;
        private readonly ICommentService _commentService;
        private readonly IAccountService _accountService;


        public CommentController(IMapper mapper,
            ILogger<CommentController> logger,
            ICommentService commentService,
            IAccountService accountService)
        {
            _mapper = mapper;
            _logger = logger;
            _commentService = commentService;
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (model != null)
                {
                    var principal = HttpContext.User;
                    if (principal != null)
                    {
                        foreach (var claim in principal.Claims)
                        {
                            model.UserId = await _accountService.GetUserIdByNicknameAsync(claim.Value);
                            await _commentService.CreateAsync(_mapper.Map<CreateOrEditCommentDto>(model));

                            return Redirect($"~/Article/ReadArticle/{model.ArticleId}");
                        }
                    }
                }

                return Redirect($"~/Article/ReadArticle/{model?.ArticleId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, Guid returnUrl)
        {
            try
            {
                var comment = await _commentService.GetCommentAsync(id);
                var model = _mapper.Map<DeleteCommentViewModel>(comment);
                model.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(DeleteCommentViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var delete = await _commentService.DeleteAsync(model.Id);
                    if (delete == null)
                    {
                        _logger.LogWarning($"{DateTime.Now}: Model is null in DeleteComment method");
                        return BadRequest();
                    }
                    return Redirect($"~/Article/ReadArticle/{model.ReturnUrl}");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Edit(Guid id, Guid returnUrl)
        {
            try
            {
                var comment = await _commentService.GetCommentAsync(id);
                var model = _mapper.Map<CommentModel>(comment);
                model.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(CommentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        await _commentService.UpdateAsync(_mapper.Map<CreateOrEditCommentDto>(model));
                    }
                    return Redirect($"~/Article/ReadArticle/{model.ReturnUrl}");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
