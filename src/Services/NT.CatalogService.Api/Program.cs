using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NT.CatalogService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Catalog Service";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://0.0.0.0:8803")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}