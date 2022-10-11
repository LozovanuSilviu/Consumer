
using System.Collections.Concurrent;
using ConsumerServer.Controllers.Models;
using Newtonsoft.Json;
using RestSharp;

namespace ConsumerServer.Services;

public class ConsumerService
{
    private readonly ConcurrentQueue<Letter> _queue;
    private readonly ILogger<ConsumerService> _logger;

    public ConsumerService(ILogger<ConsumerService> logger)
    {
        _logger = logger;
        _queue = new ConcurrentQueue<Letter>();
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

                var client = new RestClient("http://localhost:5000");
                var serialized = JsonConvert.SerializeObject(letter);
                var request = new RestRequest("/api/send/to/producer", Method.Post);
                request.AddJsonBody(serialized);
                await client.ExecuteAsync(request);
            }
        }
    }
}