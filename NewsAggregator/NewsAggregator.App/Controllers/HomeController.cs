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

        public HomeController(IMapper mapper, IArticleService articleService)
        {
            _mapper = mapper;
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            var model = (await _articleService.GetAllNewsAsync())
                .Select(article => _mapper.Map<TopNewsOnHomeScreenViewModel>(article)).ToList();

            return View(model);
        }
    }
}