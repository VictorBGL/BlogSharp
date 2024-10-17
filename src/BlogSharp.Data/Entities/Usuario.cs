namespace BlogSharp.Data.Entities
{
    public class Usuario
    {
        public Usuario()
        {

        }

        public Usuario(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
            DataCadastro = DateTime.Now;
        }

        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public bool Administrador { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public virtual ICollection<Publicacao>? Publicacoes { get; private set; }
        public virtual ICollection<Comentario>? Comentarios { get; private set; }

        public void SetAdmin()
        {
            Administrador = true;
        }
    }
}
