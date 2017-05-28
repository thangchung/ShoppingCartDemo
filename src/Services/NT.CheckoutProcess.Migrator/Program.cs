using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NT.CheckoutProcess.Infrastructure;

namespace NT.CheckoutProcess.Migrator
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static IServiceProvider _serviceProvider;

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Register services...");
                RegisterServices();

                Console.WriteLine("Migrate if there is not...");
                InitializeData().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message);
            }
        }

        private static void RegisterServices()
        {
            var containerBuilder = new ContainerBuilder();
            IServiceCollection services = new ServiceCollection();

            _configuration = LoadConfig();

            RegisterDataServices(services, containerBuilder);

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            _serviceProvider = container.Resolve<IServiceProvider>();
        }

        private static async Task InitializeData()
        {
            using (var serviceScope = _serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<CheckoutProcessDbContext>();
                if (!((RelationalDatabaseCreator) context.GetService<IDatabaseCreator>()).Exists())
                    context.Database.Migrate();
            }
        }

        private static IConfigurationRoot LoadConfig()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true);

            builder.AddEnvironmentVariables();
            return builder.Build();
        }

        private static void RegisterDataServices(IServiceCollection services, ContainerBuilder containerBuilder)
        {
            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
            var connString = _configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<CheckoutProcessDbContext>(
                options => options.UseSqlServer(connString, b => b.MigrationsAssembly(migrationsAssembly)));
        }
    }
}