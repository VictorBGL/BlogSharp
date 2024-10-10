using BlogSharp.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BlogSharp.Data.Mapping
{
    public class ComentarioMapping : IEntityTypeConfiguration<Comentario>
    {
        public void Configure(EntityTypeBuilder<Comentario> builder)
        {
            builder.ToTable("Comentario");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("ComentarioId");

            builder.HasOne(p => p.Publicacao)
                .WithMany(p => p.Comentarios)
                .HasForeignKey(p => p.PublicacaoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Autor)
                .WithMany(p => p.Comentarios)
                .HasForeignKey(p => p.AutorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
