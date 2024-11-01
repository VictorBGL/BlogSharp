using AutoMapper;
using BlogSharp.Data.Entities;
using BlogSharp.Data.Models;

namespace BlogSharp.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<PublicacaoModel, Publicacao>().ReverseMap();
            CreateMap<Publicacao, PublicacaoResponseModel>();

            CreateMap<Usuario, UsuarioResponseModel>();

            CreateMap<Comentario, ComentarioResponseModel>();
        }
    }
}
