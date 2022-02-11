using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Data;
using System.Diagnostics;

namespace NewsAggregator.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IArticleService _articleService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMapper mapper, IArticleService articleService, ILogger<HomeController> logger)
        {
            _mapper = mapper;
            _articleService = articleService;
            _logger = logger;
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
    }
}