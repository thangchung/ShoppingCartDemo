using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NT.AuditService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Audit Service";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://0.0.0.0:8806")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}