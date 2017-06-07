using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NT.CheckoutProcess.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Checkout Process";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://0.0.0.0:8804")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}