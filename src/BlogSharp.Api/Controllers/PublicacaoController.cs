using BlogSharp.Data.Data;
using BlogSharp.Data.Entities;
using BlogSharp.Data.Interfaces;
using BlogSharp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogSharp.Api.Controllers
{
    [ApiController]
    [Route("api/publicacao")]
    public class PublicacaoController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public PublicacaoController(
            ApiDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetPublicacoes()
        {
            var publicacoes = await _context.Publicacoes.OrderByDescending(p => p.DataPublicacao).ToListAsync();

            return Ok(publicacoes);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetPublicao([FromRoute] Guid id)
        {
            var publicacao = await _context.Publicacoes.FindAsync(id);

            return Ok(publicacao);
        }

        [HttpPost]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> CriarPublicacao([FromBody] PublicacaoModel model)
        {
            var usuarioId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var publicacao = new Publicacao(model.Titulo, model.Descricao, model.Imagem, Guid.Parse(usuarioId));

            _context.Publicacoes.Add(publicacao);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
