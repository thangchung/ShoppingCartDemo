using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using NT.CustomerService.Infrastructure;

namespace NT.CustomerService.Migrator
{
    public class CustomerDbContextFactory : IDbContextFactory<CustomerDbContext>
    {
        public CustomerDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(options.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{options.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var connstr = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connstr))
                throw new InvalidOperationException("Could not find a connection string named '(DefaultConnection)'.");

            if (string.IsNullOrEmpty(connstr))
                throw new InvalidOperationException($"{nameof(connstr)} is null or empty.");

            var migrationsAssembly = typeof(CustomerDbContextFactory).GetTypeInfo().Assembly.GetName().Name;
            var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
            optionsBuilder.UseSqlServer(connstr, b => b.MigrationsAssembly(migrationsAssembly));

            return new CustomerDbContext(optionsBuilder.Options);
        }
    }
}