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
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;
        private readonly IHtmlParserService _htmlParserService;
        private readonly IArticlesConcurrentService _articlesConcurrentService;

        public HomeController(IMapper mapper,
            ILogger<HomeController> logger,
            IArticleService articleService,
            ISourceService sourceService, IRssService rssService,
            IHtmlParserService htmlParserService,
            IArticlesConcurrentService articlesConcurrentService)
        {
            _mapper = mapper;
            _logger = logger;
            _articleService = articleService;
            _sourceService = sourceService;
            _rssService = rssService;
            _htmlParserService = htmlParserService;
            _articlesConcurrentService = articlesConcurrentService;
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
                return BadRequest();
            }
        }

        public async Task<IActionResult> GetNewsFromSources()
        {
            try
            {
                await _articlesConcurrentService.GetNewsFromSources();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new {ex.Message});
            }
        }
    }
}