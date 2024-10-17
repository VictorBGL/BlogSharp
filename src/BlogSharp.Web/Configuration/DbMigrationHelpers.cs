using BlogSharp.Data.Data;
using BlogSharp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace BlogSharp.Web.Configuration
{
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
            if (context.Usuarios.Any())
                return;

            var usuario = new Usuario(Guid.NewGuid(), "Admin", "admin@admin.com", string.Empty);
            usuario.SetAdmin();

            var usuarioId = Guid.NewGuid();

            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();
        }
    }
}
