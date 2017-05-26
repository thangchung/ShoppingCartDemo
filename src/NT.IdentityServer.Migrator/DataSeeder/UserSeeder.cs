using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NT.IdentityServer.Infrastructure;

namespace NT.IdentityServer.Migrator.DataSeeder
{
    public static class UserSeeder
    {
        public static async Task Seed(IdentityServerDbContext dbContext)
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

            var userStore = new UserStore<AppUser>(dbContext);
            var password = new PasswordHasher<AppUser>();
            var rootUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "root",
                Email = "root@shoppingcart.com",
                NormalizedEmail = "root@shoppingcart.com",
                NormalizedUserName = "root@shoppingcart.com",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                LockoutEnabled = true,
                ShippingAddress = new Core.SharedKernel.AddressInfo(Guid.NewGuid(), "123 Address", "HCM", "TB district", "7000", "VN")
            };

            rootUser.PasswordHash = password.HashPassword(rootUser, "root");
            await userStore.CreateAsync(rootUser);
            await userStore.AddToRoleAsync(rootUser, "admin");
        }
    }
}