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
        public IActionResult SendToConsumer(ProccessedNews news)
        {
            _logger.LogInformation($"Breaking news for your attention this is the {news.index} news with '{news.message} for today'");
            _service.Enqueue(news);
            return Ok();
        }
    }
}