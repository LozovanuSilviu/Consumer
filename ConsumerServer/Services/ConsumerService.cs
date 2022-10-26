using ConsumerServer.Controllers.Models;
using Newtonsoft.Json;
using RestSharp;

namespace ConsumerServer.Services;

public class ConsumerService
{
    private readonly Queue<News> _queue;
    private readonly ILogger<ConsumerService> _logger;
    public Mutex mutex { get; set; }
    public Mutex mutex1 { get; set; }
    public ConsumerService(ILogger<ConsumerService> logger)
    {
        _logger = logger;
        _queue = new Queue<News>();
        mutex = new Mutex();
        mutex1 = new Mutex();
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

    public void ThreadProccess()
    {
        SendFeedback();
    }

    public void Enqueue(News news)
    {
        mutex.WaitOne();
            _queue.Enqueue(news);
            mutex.ReleaseMutex();
    }

    public  Task SendFeedback()
    {
        while (true)
        {
            mutex1.WaitOne();
                if (_queue.Count !=0)
                {
                    _queue.TryDequeue(out News news);
                    var client = new RestClient("http://localhost:5000");
                    var serialized = JsonConvert.SerializeObject(news);
                    var request = new RestRequest("/api/send/to/producer", Method.Post);
                    request.AddJsonBody(serialized);
                    var response =client.ExecuteAsync(request);
                }  
                mutex1.ReleaseMutex();
        }
    }
}