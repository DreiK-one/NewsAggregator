﻿using Microsoft.AspNetCore.Mvc;

namespace NewsAggregator.App.Controllers
{
    public class CommentController : Controller
    {
        private readonly ILogger<CommentController> _logger;

        public CommentController(ILogger<CommentController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now}: Index was called");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        //TODO
    }
}
