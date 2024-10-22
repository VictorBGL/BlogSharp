using BlogSharp.Data.Entities;

namespace BlogSharp.Data.Models
{
    public class ComentarioResponseModel
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataPublicacao { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
        public virtual UsuarioResponseModel? Autor { get; set; }
    }
}
