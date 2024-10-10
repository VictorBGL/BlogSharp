using BlogSharp.Data.Data;
using BlogSharp.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSharp.Api.Controllers
{
    [ApiController]
    [Route("api/publicacao")]
    public class PublicacaoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public PublicacaoController(
            ApiDbContext context
        )
        {
            _context = context;
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
        public async Task<IActionResult> CriarPublicacao([FromBody] Publicacao publicacao)
        {
            _context.Publicacoes.Add(publicacao);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
