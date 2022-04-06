using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using System.Security.Claims;

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
        public async Task<IActionResult> CreateComment(CommentViewModel model)
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
                    foreach(var claim in principal.Claims)
                    {
                        var claimsId = claim.Value;

                        var userid = await _accountService.GetUserIdByNicknameAsync(claimsId);
                        model.UserId = userid;

                        await _commentService.CreateAsync(_mapper.Map<CreateOrEditCommentDto>(model));
                        return Redirect($"~/Article/ReadArticle/{model.ArticleId}");
                    }
                }
                
            }

            return Redirect($"~/Article/ReadArticle/{model?.ArticleId}");
        }
    }
}
