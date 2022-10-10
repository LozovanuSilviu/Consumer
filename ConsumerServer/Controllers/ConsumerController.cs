using ConsumerServer.Controllers.Models;
using ConsumerServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerServer.Controllers
{
    [ApiController]
    [Route("api")]

    public class ConsumerController : Controller
    {
        private readonly ILogger<ConsumerController> _logger;
        private readonly ConsumerService _service;

        public ConsumerController(ILogger<ConsumerController> logger, ConsumerService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("send/to/consumer")]
        public IActionResult SendToConsumer(Letter letter)
        {
            _logger.LogInformation($"Received a letter from {letter.SenderName} with messsage {letter.Message}");
            _service.Enqueue(letter);
            return Ok();
        }
    }
}