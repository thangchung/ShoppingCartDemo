using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NT.Infrastructure;
using NT.Core.UserContext;

namespace BlogCore.MigrationConsole.SeedData
{
    public static class UserSeeder
    {
        public static async Task Seed(AppDbContext dbContext)
        {
            var roleStore = new RoleStore<IdentityRole>(dbContext);
            await roleStore.CreateAsync(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "user",
                NormalizedName = "user"
            });
            await roleStore.CreateAsync(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "admin"
            });

            var userStore = new UserStore<User>(dbContext);
            var password = new PasswordHasher<User>();
            var rootUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "root",
                Email = "root@shoppingcart.com",
                NormalizedEmail = "root@shoppingcart.com",
                NormalizedUserName = "root@shoppingcart.com",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                LockoutEnabled = true
            };

            rootUser.PasswordHash = password.HashPassword(rootUser, "root");
            await userStore.CreateAsync(rootUser);
            await userStore.AddToRoleAsync(rootUser, "admin");
        }
    }
}