using Microsoft.AspNetCore.Mvc;

namespace NewsAggregator.App.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
