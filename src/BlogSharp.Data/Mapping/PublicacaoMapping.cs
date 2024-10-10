using BlogSharp.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BlogSharp.Data.Mapping
{
    public class PublicacaoMapping : IEntityTypeConfiguration<Publicacao>
    {
        public void Configure(EntityTypeBuilder<Publicacao> builder)
        {
            builder.ToTable("Publicacao");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("PublicacaoId");

            builder.HasOne(p => p.Autor)
                .WithMany(p => p.Publicacoes)
                .HasForeignKey(p => p.AutorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
