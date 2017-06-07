using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NT.CustomerService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Customer Service";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://0.0.0.0:8801")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}