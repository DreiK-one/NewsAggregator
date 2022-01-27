using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.App.Models;
using NewsAggregator.Data;
using System.Diagnostics;

namespace NewsAggregator.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly NewsAggregatorContext _db;

        public HomeController(NewsAggregatorContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _db.Articles
                .OrderByDescending(article => article.Coefficient)
                .Select(article => new TopNewsOnHomeScreenViewModel()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Discription = article.Description,
                    CreationDate = article.CreationDate
                }).ToListAsync();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}