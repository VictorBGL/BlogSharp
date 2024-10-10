namespace BlogSharp.Data.Entities
{
    public class Comentario
    {
        public Guid Id { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataPublicacao { get; private set; }
        public DateTime DataUltimaAtualizacao { get; private set; }
        public Guid? AutorId { get; private set; }
        public Guid PublicacaoId { get; private set; }
        public virtual Publicacao Publicacao { get; private set; }
        public virtual Usuario? Autor { get; private set; }
    }
}
