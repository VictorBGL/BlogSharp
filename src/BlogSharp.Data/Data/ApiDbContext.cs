using BlogSharp.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogSharp.Data.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base (options) 
        {
            
        }

        public DbSet<Publicacao> Publicacoes { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
