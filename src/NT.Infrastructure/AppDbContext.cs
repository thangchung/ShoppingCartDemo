using Microsoft.EntityFrameworkCore;
using NT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NT.Core.UserContext;

namespace NT.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typeToRegisters = new List<Type>();
            typeToRegisters.AddRange(typeof(EntityBase).GetTypeInfo().Assembly.DefinedTypes.Select(t => t.AsType()));
            var entityTypes = typeToRegisters.Where(x => !x.GetTypeInfo().IsAbstract
                                                         && x.GetTypeInfo().BaseType == typeof(EntityBase));

            // temporary to concanate with s at the end, but need to have a way to translate it to a plural noun
            foreach (var type in entityTypes)
                modelBuilder.Entity(type).ToTable($"{type.Name}s", "cart");

            typeToRegisters = new List<Type>();
            typeToRegisters.AddRange(typeof(ValueObject).GetTypeInfo().Assembly.DefinedTypes.Select(t => t.AsType()));
            var sharedTypes = typeToRegisters.Where(x => !x.GetTypeInfo().IsAbstract
                                                         && x.GetTypeInfo().BaseType == typeof(ValueObject));

            // temporary to concanate with s at the end, but need to have a way to translate it to a plural noun
            foreach (var type in sharedTypes)
                modelBuilder.Entity(type).ToTable($"{type.Name}s", "shared");

            base.OnModelCreating(modelBuilder);
        }
    }
}
