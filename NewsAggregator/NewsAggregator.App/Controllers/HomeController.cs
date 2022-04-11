using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Data;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace NewsAggregator.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly IConfiguration _configuration;

        public HomeController(IMapper mapper,
            ILogger<HomeController> logger,
            IArticleService articleService, 
            IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _articleService = articleService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                if (page < 1)
                {
                    page = 1;
                }

                var pageSize = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

                var articles = (await _articleService.GetNewsByPageAsync(page - 1))
                    .Select(article => _mapper.Map<AllNewsOnHomeScreenViewModel>(article))
                    .OrderByDescending(article => article.CreationDate)
                    .ToList();

                var articlesCount = Convert.ToInt32(Math.Ceiling((double)
                    (await _articleService.GetAllNewsAsync()).Count() / pageSize));

                var pager = new Pager(articlesCount, page, pageSize);

                var model = new ArticlesByPagesViewModel()
                {
                    NewsList = articles,
                    Pager = pager
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { ex.Message });
            }
        }  
    }
}