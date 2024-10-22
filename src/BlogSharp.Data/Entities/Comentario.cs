namespace BlogSharp.Data.Entities
{
    public class Comentario
    {
        public Comentario()
        {

        }

        public Comentario(string descricao, Guid autorId, Guid publicacaoId)
        {
            Descricao = descricao;
            AutorId = autorId;
            PublicacaoId = publicacaoId;
            DataPublicacao = DateTime.Now;
        }

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
