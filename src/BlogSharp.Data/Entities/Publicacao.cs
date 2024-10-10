namespace BlogSharp.Data.Entities
{
    public class Publicacao
    {
        public Guid Id { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public string? Imagem { get; private set; }
        public DateTime DataPublicacao { get; private set; }
        public DateTime? DataUltimaAtualizacao { get; private set; }
        public Guid AutorId { get; private set; }
        public virtual Usuario Autor { get; private set; }
        public virtual ICollection<Comentario>? Comentarios { get; private set; }
    }
}
