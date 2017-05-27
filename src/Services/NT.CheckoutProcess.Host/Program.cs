using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NT.CheckoutProcess.Infrastructure;
using NT.Core;
using NT.Infrastructure.MessageBus;
using NT.Infrastructure.MessageBus.Event;
using NT.Infrastructure.MessageBus.RabbitMq;

namespace NT.CheckoutProcess.Host
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
                Console.WriteLine("Listening events...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message);
            }
        }

        private static void RegisterServices()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true);

            builder.AddEnvironmentVariables();
            _configuration = builder.Build();

            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
            var connString = _configuration.GetConnectionString("DefaultConnection");

            var containerBuilder = new ContainerBuilder();
            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<CheckoutProcessDbContext>(
                options => options.UseSqlServer(connString, b => b.MigrationsAssembly(migrationsAssembly)));

            containerBuilder.RegisterGeneric(typeof(GenericEfRepository<>))
                .As(typeof(IRepository<>));

            // RabbitMq
            containerBuilder.RegisterAssemblyTypes(typeof(CheckoutProcessDbContext).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            containerBuilder.Register(x => new RabbitMqPublisher(_configuration.GetValue<string>("Rabbitmq"), "order.exchange"))
                .As<IEventBus>()
                .SingleInstance();

            containerBuilder.RegisterInstance(new RabbitMqSubscriber(_configuration.GetValue<string>("Rabbitmq"), "order.exchange", "order.queue"))
                .Named<IEventSubscriber>("EventSubscriber");

            containerBuilder.Register(x =>
                new EventConsumer(
                    x.ResolveNamed<IEventSubscriber>("EventSubscriber"),
                    (IEnumerable<IMessageHandler>)x.Resolve(typeof(IEnumerable<IMessageHandler>))
                )
            ).As<IEventConsumer>();

            containerBuilder.Populate(services);
            _serviceProvider = containerBuilder.Build().Resolve<IServiceProvider>();

            // listen the event from others
            _serviceProvider.GetService<IEventConsumer>().Subscriber.Subscribe();
        }
    }
}
