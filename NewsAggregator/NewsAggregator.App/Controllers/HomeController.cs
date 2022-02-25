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
        private readonly IHtmlParserService _htmlParserService;

        public HomeController(IMapper mapper,
            IArticleService articleService,
            ILogger<HomeController> logger,
            ISourceService sourceService, IRssService rssService, 
            IHtmlParserService htmlParserService)
        {
            _mapper = mapper;
            _articleService = articleService;
            _logger = logger;
            _sourceService = sourceService;
            _rssService = rssService;
            _htmlParserService = htmlParserService;
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
                var rssUrls = await _sourceService.GetRssUrlsAsync();
                var concurrentDictionary = new ConcurrentDictionary<string, RssArticleDto?>();
                var result = Parallel.ForEach(rssUrls, dto =>
                {
                    _rssService.GetArticlesInfoFromRss(dto.RssUrl).AsParallel().ForAll(articleDto 
                        => concurrentDictionary.TryAdd(articleDto.Url, articleDto));
                });

                var extArticlesUrls = await _articleService.GetAllExistingArticleUrls();

                Parallel.ForEach(extArticlesUrls.Where(url => concurrentDictionary.ContainsKey(url)), 
                    s => concurrentDictionary.Remove(s, out var dto));

                foreach (var rssArticleDto in concurrentDictionary)
                {
                    var body = await _htmlParserService.GetArticleContentFromUrlAsync(rssArticleDto.Key);
                }

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