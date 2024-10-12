using BlogSharp.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace BlogSharp.Data.Models
{
    public class PublicacaoModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public required string Descricao { get; set; }

        public string? Imagem { get; set; }
    }
}
