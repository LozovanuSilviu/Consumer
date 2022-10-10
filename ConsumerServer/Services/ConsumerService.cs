using System.Text;
using System.Text.Json.Serialization;
using ConsumerServer.Controllers.Models;
using Newtonsoft.Json;

namespace ConsumerServer.Services;

public class ConsumerService
{
    private readonly Queue<Letter> _queue;
    private readonly ILogger<ConsumerService> _logger;

    public ConsumerService(ILogger<ConsumerService> logger)
    {
        _logger = logger;
        _queue = new Queue<Letter>();
        Run();
    }

    public async Task Run()
    {
        for (int i = 0; i < 3; i++)
        {
            Task.Run(SendBackLetters);
        }
    }

    public void Enqueue(Letter letter)
    {
        _queue.Enqueue(letter);
    }

    public async Task SendBackLetters()
    {
        while (true)
        {
            if (_queue.Count !=0)
            {
                _queue.TryDequeue(out Letter letter);

                var client = new HttpClient();
                var serialized = JsonConvert.SerializeObject(letter);
                await client.PostAsync("http://localhost:5019/api/sent/to/producer",
                    new StringContent(serialized, Encoding.UTF32, "applications/json"));
            }
        }
    }
}