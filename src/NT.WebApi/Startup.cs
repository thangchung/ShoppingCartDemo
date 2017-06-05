using System;
using System.Reflection;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityServer4.Models;
using Microphone.AspNet;
using Microphone.Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NT.Infrastructure.AspNetCore;
using NT.Infrastructure.MessageBus;
using NT.Infrastructure.MessageBus.Event;
using NT.Infrastructure.MessageBus.RabbitMq;
using Swashbuckle.AspNetCore.Swagger;

namespace NT.WebApi
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
            Environment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddAuthorization();

            // Add framework services.
            services.AddMvc();
            services.AddMicrophone<ConsulProvider>();
            services.Configure<ConsulOptions>(o =>
            {
                o.Host = "localhost";
            });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Demo",
                    Version = "v1",
                    Description = "Demo APIs"
                });
                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    TokenUrl = "http://localhost:9999/connect/token",
                    AuthorizationUrl = "http://localhost:9999/connect/authorize",
                    Scopes = new Dictionary<string, string>
                    {
                        {"customer_service", "Customer Service."}
                    }
                });
            });

            builder.RegisterType<RestClient>()
                .AsSelf();

            // RabbitMq
            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.Register(x => new RabbitMqPublisher(Configuration.GetValue<string>("Rabbitmq"), "order.exchange"))
                .As<IEventBus>()
                .SingleInstance();

            builder.RegisterInstance(new RabbitMqSubscriber(Configuration.GetValue<string>("Rabbitmq"), "order.exchange", "order.queue"))
                .Named<IEventSubscriber>("EventSubscriber")
                .SingleInstance();

            builder.Register(x =>
                new EventConsumer(
                    x.ResolveNamed<IEventSubscriber>("EventSubscriber"),
                    (IEnumerable<IMessageHandler>)x.Resolve(typeof(IEnumerable<IMessageHandler>))
                )
            ).As<IEventConsumer>()
            .SingleInstance();

            builder.Populate(services);
            var serviceProvider = builder.Build().Resolve<IServiceProvider>();
            serviceProvider.GetService<IEventConsumer>().Subscriber.Subscribe();
            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                AuthenticationScheme = "Bearer",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                Authority = "http://localhost:9999",
                SaveToken = true,
                AllowedScopes = new[] {"customer_service"},
                RequireHttpsMetadata = false,
                JwtBearerEvents = new JwtBearerEvents
                {
                    OnTokenValidated = OnTokenValidated
                }
            });

            app.UseStaticFiles().UseCors("CorsPolicy");

            app.UseMvc()
                .UseMicrophone("api_gateway", "1.0");

            app.UseSwagger().UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo APIs");
                    c.ConfigureOAuth2("swagger", "secret".Sha256(), "swagger", "swagger");
                });
        }

        private static async Task OnTokenValidated(TokenValidatedContext context)
        {
            // get current principal
            var principal = context.Ticket.Principal;

            // get current claim identity
            var claimsIdentity = context.Ticket.Principal.Identity as ClaimsIdentity;

            // build up the id_token and put it into current claim identity
            var headerToken =
                context.Request.Headers["Authorization"][0].Substring(context.Ticket.AuthenticationScheme.Length + 1);
            claimsIdentity?.AddClaim(new Claim("id_token", headerToken));


            await Task.FromResult(0);
        }
    }
}