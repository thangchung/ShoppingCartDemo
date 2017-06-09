using System;
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
using NT.AuditService.Core;
using NT.AuditService.Infrastructure;
using NT.Core;
using NT.Core.Events;
using NT.Infrastructure;
using RawRabbit.vNext;

namespace NT.AuditService.Api
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

            services.AddDbContext<AuditDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Core & Infra register
            builder.RegisterGeneric(typeof(GenericEfRepository<>))
                .As(typeof(IRepository<>));

            // Add framework services.
            services.AddMvc();
            services.AddRawRabbit(cfg => cfg.AddJsonFile("rawrabbit.json"));

            builder.Populate(services);
            var container = builder.Build();
            var client = BusClientFactory.CreateDefault();

            client.SubscribeAsync<AddAuditEvent>(async (msg, context) =>
            {
                await Task.Run(() =>
                {
                    var repo = container.Resolve<IRepository<AuditInfo>>();
                    var audit = repo.AddAsync(
                        new AuditInfo
                        {
                            ServiceName = msg.ServiceName,
                            MethodName = msg.MethodName,
                            ActionMessage = msg.ActionMessage,
                            Created = DateTime.UtcNow
                        });

                    if (audit == null)
                        throw new Exception("Cannot add a new audit info.");
                });
            });

            return container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseMicrophone("audit_service", "1.0");
        }
    }
}