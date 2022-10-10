using System.Net;
using Lab1.Models;

namespace Lab1;

public class ConsumerServer
{
    public bool Running;
    
    private HttpListener Listener;

    private Queue<HttpListenerRequest> Requests;
    private const string url = "http://localhost:8000/";

    private Thread thread;
    public ConsumerServer()
    {
        Listener = new HttpListener();
        Listener.Prefixes.Add(url);
    }

    public void Start()
    {
         thread = new Thread(Run);
        thread.Start();
    }
    
    public void DisplayRequestInfo(HttpListenerRequest req)
    {
        Console.WriteLine("Endpoint:" + req.LocalEndPoint);
        Console.WriteLine("Method: " + req.HttpMethod);
        Console.WriteLine("Payload: ");
    }

    private void Run()
    {
        Running = true;
        Listener.Start();
        Console.WriteLine("Listening for connections on {0}", url);

        while (Running)
        {
            HttpListenerContext context =await Listener.GetContextAsync();
            Requests.Enqueue(context.Request);
            HandleRequests(Requests);
        }
        Listener.Stop();
    }

    public void Stop()
    {
        Running = false;
        thread.Abort();
    }
    

    private void HandleRequests(Queue<HttpListenerRequest> context)
    {
    }
}