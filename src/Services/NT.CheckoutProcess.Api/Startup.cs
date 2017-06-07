using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
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
using NT.Core.Events;
using NT.Infrastructure.AspNetCore;
using NT.Infrastructure.MessageBus;
using NT.Infrastructure.MessageBus.Event;
using NT.Infrastructure.MessageBus.RabbitMq;
using RawRabbit.vNext;

namespace NT.CheckoutProcess.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
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

            builder.RegisterType<CheckoutSaga>()
                .AsSelf();

            // RabbitMq
            /*builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.Register(x => new RabbitMqPublisher(Configuration.GetValue<string>("Rabbitmq"), "order.exchange"))
                .As<IEventBus>()
                .SingleInstance();

            builder.RegisterInstance(new RabbitMqSubscriber(Configuration.GetValue<string>("Rabbitmq"),
                    "order.exchange", "order.queue"))
                .Named<IEventSubscriber>("EventSubscriber")
                .SingleInstance();

            builder.Register(x =>
                    new EventConsumer(
                        x.ResolveNamed<IEventSubscriber>("EventSubscriber"),
                        (IEnumerable<IMessageHandler>) x.Resolve(typeof(IEnumerable<IMessageHandler>))
                    )
                ).As<IEventConsumer>()
                .SingleInstance();*/

            // Add framework services.
            services.AddMvc();

            services.AddRawRabbit(cfg => cfg.AddJsonFile("rawrabbit.json"));

            builder.Populate(services);
            var container = builder.Build();
            var serviceProvider = container.Resolve<IServiceProvider>();
            // serviceProvider.GetService<IEventConsumer>().Subscriber.Subscribe();
            var client = BusClientFactory.CreateDefault();

            client.SubscribeAsync<CheckoutEvent>(async (msg, context) =>
            {
                var saga = container.Resolve<CheckoutSaga>();
                await Task.Run(() => saga.Checkout(new Guid(), msg.OrderId));
            });

            client.SubscribeAsync<PaymentAcceptedEvent>(async (msg, context) =>
            {
                var saga = container.Resolve<CheckoutSaga>();
                await Task.Run(() => saga.PaymentAccepted(msg.SagaId));
            });

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