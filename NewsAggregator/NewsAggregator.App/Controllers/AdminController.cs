﻿using Microsoft.AspNetCore.Mvc;

namespace NewsAggregator.App.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
