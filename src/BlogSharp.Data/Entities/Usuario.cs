namespace BlogSharp.Data.Entities
{
    public class Usuario
    {
        public Usuario(Guid id, string nome, string email, string? imagem)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Imagem = imagem;
            DataCadastro = DateTime.Now;
        }

        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public bool Administrador { get; private set; }
        public bool Ativo { get; private set; }
        public string? Imagem { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public virtual ICollection<Publicacao>? Publicacoes { get; private set; }
        public virtual ICollection<Comentario>? Comentarios { get; private set; }
    }
}
