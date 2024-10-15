
using AutoMapper;
using BlogSharp.Data.Entities;
using BlogSharp.Data.Models;

namespace BlogSharp.Web.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<PublicacaoModel, Publicacao>();
            CreateMap<Publicacao, PublicacaoResponseModel>();

            CreateMap<Usuario, UsuarioResponseModel>();
        }
    }
}
