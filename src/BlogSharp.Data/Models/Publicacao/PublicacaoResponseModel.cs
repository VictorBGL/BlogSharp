namespace BlogSharp.Data.Models
{
    public class PublicacaoResponseModel
    {
        public Guid Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Imagem { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public UsuarioResponseModel Autor { get; set; }
    }
}
