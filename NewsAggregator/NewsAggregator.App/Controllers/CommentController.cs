using Microsoft.AspNetCore.Mvc;

namespace NewsAggregator.App.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
