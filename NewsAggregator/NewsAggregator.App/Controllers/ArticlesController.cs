using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;

namespace NewsAggregator.App.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly NewsAggregatorContext _db;

        public ArticlesController(NewsAggregatorContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _db.Articles
                .OrderByDescending(article => article.CreationDate).ToListAsync();

            return View(articles);
        }
    }
}
