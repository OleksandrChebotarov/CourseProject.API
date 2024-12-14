using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CourseProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeController : ControllerBase
    {
        private readonly ILogger<TimeController> _logger;

        public TimeController(ILogger<TimeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetServerTime()
        {
            _logger.LogInformation("Received request to get server time.");

            var serverTime = DateTime.Now;
            _logger.LogInformation("Returning server time: {Time}", serverTime);

            return Ok(new { Time = serverTime });
        }
    }
}