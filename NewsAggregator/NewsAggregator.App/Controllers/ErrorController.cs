using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NewsAggregator.App.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        private readonly TelemetryClient _telemetryClient;

        public ErrorController(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("500")]
        public IActionResult Error500()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            _telemetryClient.TrackException(exceptionHandlerPathFeature.Error);
            _telemetryClient.TrackEvent("Error.ServerError", new Dictionary<string, string>
            {
                ["originalPath"] = exceptionHandlerPathFeature.Path,
                ["error"] = exceptionHandlerPathFeature.Error.Message
            });

            return View();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("404")]
        public IActionResult Error404()
        {
            string originalPath = "unknown";
            if (HttpContext.Items.ContainsKey("originalPath"))
            {
                originalPath = HttpContext.Items["originalPath"] as string;
            }

            _telemetryClient.TrackEvent("Error.PageNotFound", new Dictionary<string, string>
            {
                ["originalPath"] = originalPath
            });

            return View();
        }
    }
}
