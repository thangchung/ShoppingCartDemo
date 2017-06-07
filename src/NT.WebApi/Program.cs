using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NT.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "API Gateway";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://0.0.0.0:8888")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}