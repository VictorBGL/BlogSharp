using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogSharp.Data.Models
{
    public class PublicacaoModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public required string Descricao { get; set; }

        public string? Imagem { get; set; }
        public IFormFile? ImagemFile { get; set; }
    }
}
