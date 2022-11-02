using ConsumerServer.Controllers.Models;
using Newtonsoft.Json;
using RestSharp;

namespace ConsumerServer.Services;

public class ConsumerService
{
    private readonly Queue<ProccessedNews> _queue;
    private readonly ILogger<ConsumerService> _logger;
    public Mutex mutex { get; set; }
    public ConsumerService(ILogger<ConsumerService> logger)
    {
        _logger = logger;
        _queue = new Queue<ProccessedNews>();
        mutex = new Mutex();
        Run();
    }

    public  Task Run()
    {
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(3000);
            Task.Run(SendFeedback);
        }

        return Task.CompletedTask;
    }

    public void Enqueue(ProccessedNews news)
    {
        mutex.WaitOne();
            _queue.Enqueue(news);
            mutex.ReleaseMutex();
    }

    public  Task SendFeedback()
    {
        while (true)
        {
            mutex.WaitOne();
                if (_queue.Count !=0)
                {
                    _queue.TryDequeue(out ProccessedNews news);
                    var client = new RestClient("http://localhost:5086");
                    var serialized = JsonConvert.SerializeObject(news);
                    var request = new RestRequest("/api/send/back/to/aggregator", Method.Post);
                    request.AddJsonBody(serialized); 
                    client.ExecuteAsync(request);
                    
                }  
                mutex.ReleaseMutex();
        }
    }
}