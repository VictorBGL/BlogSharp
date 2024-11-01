namespace BlogSharp.Data.Models
{
    public class UsuarioLoginResponseModel
    {
        public bool Authenticated { get; set; }
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioResponseModel UsuarioToken { get; set; }
    }
}
