using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RemoteDockService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .UseWindowsService(options => options.ServiceName = "RemoteDockService")
                .ConfigureServices(services => services.AddHostedService<ServerWorker>())
                .Build();
            await host.RunAsync();
        }
    }

    public class ServerWorker : BackgroundService
    {
        private HttpListener? _listener;
        private const int Port = 8080;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://+:{Port}/");
            try
            {
                _listener.Start();
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var context = await _listener.GetContextAsync().WaitAsync(stoppingToken);
                        _ = HandleRequest(context);
                    }
                    catch (OperationCanceledException) { break; }
                    catch (HttpListenerException) { break; }
                }
            }
            finally { _listener?.Close(); }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            var response = context.Response;
            response.Headers.Add("Content-Type", "application/json");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            var path = context.Request.Url?.AbsolutePath.ToLower() ?? "/";
            try
            {
                switch (path)
                {
                    case "/status":
                        await SendJson(response, new { online = true, hostname = Environment.MachineName });
                        break;
                    case "/sleep":
                        await SendJson(response, new { success = true });
                        System.Diagnostics.Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0");
                        break;
                    default:
                        await SendJson(response, new { error = "Not found" }, 404);
                        break;
                }
            }
            catch { }
        }

        private static async Task SendJson(HttpListenerResponse response, object data, int statusCode = 200)
        {
            var json = JsonSerializer.Serialize(data);
            var buffer = Encoding.UTF8.GetBytes(json);
            response.StatusCode = statusCode;
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer);
            response.Close();
        }
    }
}