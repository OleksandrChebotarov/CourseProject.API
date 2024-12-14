using CourseProject.BLL.Interfaces;
using CourseProject.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CourseProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndicatorController : ControllerBase
    {
        private readonly IIndicatorService _service;
        private readonly IHubContext<IndicatorHub> _hubContext;
        private readonly ILogger<IndicatorController> _logger;

        public IndicatorController(IIndicatorService service, IHubContext<IndicatorHub> hubContext, ILogger<IndicatorController> logger)
        {
            _service = service;
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetIndicators()
        {
            _logger.LogInformation("Fetching all indicators");
            var indicators = await _service.GetIndicators();

            _logger.LogInformation("Successfully fetched {Count} indicators", indicators.Count);
            return Ok(indicators);  
        }

        [HttpPost]
        public async Task<ActionResult> CreateIndicator(IndicatorModel model)
        {
            _logger.LogInformation("Creating a new indicator with name {Name}", model.Name);
            var id = await _service.CreateIndicator(model);

            _logger.LogInformation("Successfully created indicator with ID {Id}", id);
            return Ok(id);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateIndicatorValue(UpdateIndicatorValueModel model)
        {
            _logger.LogInformation("Updating value of indicator {Id} to {Value}", model.Id, model.Value);
            await _service.UpdateIndicatorValue(model);
            await _hubContext.Clients.All.SendAsync("receive", model.Id.ToString(), model.Value);

            _logger.LogInformation("Successfully updated indicator {Id}", model.Id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIndicator(Guid id)
        {
            _logger.LogInformation("Deleting indicator with ID {Id}", id);
            await _service.DeleteIndicator(id);

            _logger.LogInformation("Successfully deleted indicator {Id}", id);
            return NoContent();
        }
    }
}
