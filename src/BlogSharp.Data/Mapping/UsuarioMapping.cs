using BlogSharp.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BlogSharp.Data.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("UsuarioId");
        }
    }
}
