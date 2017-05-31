using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NT.AuditService.Core;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.AuditService.Infrastructure
{
    public class AuditDbContext : DbContext
    {
        public AuditDbContext(DbContextOptions<AuditDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = new List<Type>
            {
                typeof(AuditInfo)
            };

            var valueTypes = new List<Type>();

            base.OnModelCreating(modelBuilder.RegisterTypes(entityTypes, valueTypes, "audit"));
        }
    }
}