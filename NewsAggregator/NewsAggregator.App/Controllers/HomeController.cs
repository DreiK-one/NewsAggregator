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
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly ILogger<HomeController> _logger;
        private readonly IRssService _rssService;

        public HomeController(IMapper mapper,
            IArticleService articleService,
            ILogger<HomeController> logger,
            ISourceService sourceService, IRssService rssService)
        {
            _mapper = mapper;
            _articleService = articleService;
            _logger = logger;
            _sourceService = sourceService;
            _rssService = rssService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: Index was called");

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
                _logger.LogInformation($"{DateTime.Now}: GetNewsFromSources was called");

                var rssUrls = await _sourceService.GetRssUrlsAsync();
                var concurrentBag = new ConcurrentBag<RssArticleDto>();
                var result = Parallel.ForEach(rssUrls, dto =>
                {
                    _rssService.GetArticlesInfoFromRss(dto.RssUrl).AsParallel().ForAll(articleDto => concurrentBag.Add(articleDto));
                });

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return StatusCode(500, new {ex.Message});
            }
        }
    }
}