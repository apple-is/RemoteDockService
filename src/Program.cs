using System.Net;
using System.Text;
using System.Text.Json;

namespace RemoteDockService
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://+:8080/");
            
            try
            {
                listener.Start();
                Console.WriteLine("Server läuft auf Port 8080");

                while (true)
                {
                    var context = listener.GetContext();
                    ThreadPool.QueueUserWorkItem(_ => HandleRequest(context));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
                Console.ReadLine();
            }
        }

        static void HandleRequest(HttpListenerContext context)
        {
            var response = context.Response;
            response.Headers.Add("Content-Type", "application/json");
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            var path = context.Request.Url?.AbsolutePath.ToLower() ?? "/";

            switch (path)
            {
                case "/status":
                    SendJson(response, new { online = true, hostname = Environment.MachineName });
                    break;
                case "/sleep":
                    SendJson(response, new { success = true });
                    Thread.Sleep(500);
                    System.Diagnostics.Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0");
                    break;
                default:
                    SendJson(response, new { error = "Not found" }, 404);
                    break;
            }
        }

        static void SendJson(HttpListenerResponse response, object data, int statusCode = 200)
        {
            var json = JsonSerializer.Serialize(data);
            var buffer = Encoding.UTF8.GetBytes(json);
            response.StatusCode = statusCode;
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer);
            response.Close();
        }
    }
}