using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microphone.AspNet;
using Microphone.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NT.CheckoutProcess.Infrastructure;
using NT.Core;
using NT.Infrastructure.AspNetCore;
using NT.Infrastructure.MessageBus;
using NT.Infrastructure.MessageBus.Event;
using NT.Infrastructure.MessageBus.RabbitMq;

namespace NT.CheckoutProcess.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            services.AddMicrophone<ConsulProvider>();

            services.AddDbContext<CheckoutProcessDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Core & Infra register
            builder.RegisterGeneric(typeof(GenericEfRepository<>))
                .As(typeof(IRepository<>));

            builder.RegisterType<RestClient>()
                .AsSelf();

            builder.RegisterType<CheckoutWorkflow>()
                .AsSelf();

            // RabbitMq
            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.Register(x => new RabbitMqPublisher(Configuration.GetValue<string>("Rabbitmq"), "order.exchange"))
                .As<IEventBus>()
                .SingleInstance();

            builder.RegisterInstance(new RabbitMqSubscriber(Configuration.GetValue<string>("Rabbitmq"), "order.exchange", "order.queue"))
                .Named<IEventSubscriber>("EventSubscriber");

            builder.Register(x =>
                new EventConsumer(
                    x.ResolveNamed<IEventSubscriber>("EventSubscriber"),
                    (IEnumerable<IMessageHandler>)x.Resolve(typeof(IEnumerable<IMessageHandler>))
                )
            ).As<IEventConsumer>();

            // Add framework services.
            services.AddMvc();

            builder.Populate(services);
            var serviceProvider = builder.Build().Resolve<IServiceProvider>();
            serviceProvider.GetService<IEventConsumer>().Subscriber.Subscribe();
            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseMicrophone("checkout_service", "1.0");
        }
    }
}
