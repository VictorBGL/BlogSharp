using BlogSharp.Data.Models;

namespace BlogSharp.Web.Models
{
    public class PublicacaoComentarioViewModel
    {
        public PublicacaoResponseModel Publicacao { get; set; }
        public ComentarioModel Comentario { get; set; }
        public Guid PublicacaoId { get; set; }
    }
}
