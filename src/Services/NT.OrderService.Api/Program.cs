using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NT.OrderService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Order Service";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://0.0.0.0:8802")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}