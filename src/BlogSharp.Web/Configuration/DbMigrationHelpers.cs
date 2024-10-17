using BlogSharp.Data.Data;
using BlogSharp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace BlogSharp.Web.Configuration
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Docker")) 
            {
                await context.Database.MigrateAsync();

                await EnsureSeedProducts(context);
            }
        }

        public static async Task EnsureSeedProducts(ApiDbContext context)
        {
            if (context.Users.Any()) 
                return;

            var userId = Guid.NewGuid();

            await context.Users.AddAsync(new IdentityUser
            {
                Id = userId.ToString(),
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEJfnkXwMwUa7tr3NITeoGPnAjCbvkd5y2TdQ/wglcpCinkNGSF30kpgTIH3WsCTCkg==",
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            });

            await context.SaveChangesAsync();


            if (context.Usuarios.Any())
                return;

            var usuario = new Usuario(userId, "Admin", "admin@admin.com");
            usuario.SetAdmin();

            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();
        }
    }
}
