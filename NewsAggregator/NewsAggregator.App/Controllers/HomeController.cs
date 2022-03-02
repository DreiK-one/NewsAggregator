using AutoMapper;
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

        public HomeController(IMapper mapper,
            ILogger<HomeController> logger,
            IArticleService articleService)
        {
            _mapper = mapper;
            _logger = logger;
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = (await _articleService.GetAllNewsAsync())
                .Select(article => _mapper.Map<AllNewsOnHomeScreenViewModel>(article))
                .ToList();

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