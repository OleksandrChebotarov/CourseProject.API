using CourseProject.BLL.Interfaces;
using CourseProject.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CourseProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackgroundImageController : ControllerBase
    {
        private readonly IBackgroundImageService _service;
        private readonly ILogger<BackgroundImageController> _logger;

        public BackgroundImageController(IBackgroundImageService service, ILogger<BackgroundImageController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBackGroundImage(int id)
        {
            _logger.LogInformation("Fetching background image with ID {Id}", id);
            var image = await _service.GetBackgroundImageAsync(id);

            if(image == null)
            {
                _logger.LogWarning("Image with ID {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully fetched image with ID {Id}", id);
            return File(image.Bytes, "image/jpeg");
        }

        [HttpPost("upload-image")]
        public async Task<ActionResult> UploadImageAsync(IFormFile image)
        {
            _logger.LogInformation("Uploading a new background image");
            var memoryStream = new MemoryStream();
            image.CopyTo(memoryStream);
            await _service.UploadImageAsync(memoryStream);

            _logger.LogInformation("Successfully uploaded background image");
            return NoContent();
        }
    }
}
