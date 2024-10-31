namespace BlogSharp.Data.Entities
{
    public class Publicacao
    {
        public Publicacao()
        {
             
        }

        public Publicacao(string titulo, string descricao, string? imagem, Guid autorId)
        {
            Titulo = titulo;
            Descricao = descricao;
            Imagem = imagem;
            AutorId = autorId;
            DataPublicacao = DateTime.Now;
        }

        public Guid Id { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public string? Imagem { get; private set; }
        public DateTime DataPublicacao { get; private set; }
        public DateTime? DataUltimaAtualizacao { get; private set; }
        public Guid AutorId { get; private set; }
        public virtual Usuario Autor { get; private set; }
        public virtual ICollection<Comentario>? Comentarios { get; private set; }

        public void CriarPublicacao(Guid autorId)
        {
            AutorId = autorId;
            DataPublicacao = DateTime.Now;
        }

        public void Atualizar(Publicacao publicacao)
        {
            Titulo = publicacao.Titulo;
            Descricao = publicacao.Descricao;
            Imagem = publicacao.Imagem;
            DataUltimaAtualizacao = DateTime.Now;
        }
    }
}
