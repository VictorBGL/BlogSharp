using AutoMapper;
using BlogSharp.Data.Data;
using BlogSharp.Data.Entities;
using BlogSharp.Data.Interfaces;
using BlogSharp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSharp.Api.Controllers
{
    [ApiController]
    [Route("api/publicacao")]
    public class PublicacaoController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly IAspnetUser _aspnetUser;

        public PublicacaoController(
            ApiDbContext context, IHttpContextAccessor accessor, IMapper mapper, IAspnetUser aspnetUser)
        {
            _context = context;
            _accessor = accessor;
            _mapper = mapper;
            _aspnetUser = aspnetUser;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetPublicacoes()
        {
            var publicacoes = await _context.Publicacoes.Include(p => p.Autor).OrderByDescending(p => p.DataPublicacao).ToListAsync();

            var resultado = _mapper.Map<List<PublicacaoResponseModel>>(publicacoes);

            return Ok(resultado);
        }

        [Authorize]
        [HttpGet("usuario")]
        [Produces("application/json")]
        public async Task<IActionResult> GetPublicacoesPorUsuarioLogado()
        {
            var userId = _aspnetUser.GetUserId();

            var publicacoesUsuario = await _context.Publicacoes.Include(p => p.Autor).OrderByDescending(p => p.DataPublicacao).Where(p => p.AutorId == Guid.Parse(userId)).ToListAsync();
            var resultado = _mapper.Map<List<PublicacaoResponseModel>>(publicacoesUsuario);
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetPublicao([FromRoute] Guid id)
        {
            var post = await _context.Publicacoes
                                            .Include(p => p.Autor)
                                            .Include(p => p.Comentarios.OrderByDescending(x => x.DataPublicacao))
                                                .ThenInclude(p => p.Autor)
                                            .FirstOrDefaultAsync(p => p.Id == id);

            var publicacao = _mapper.Map<PublicacaoResponseModel>(post);

            return Ok(publicacao);
        }

        [HttpPost]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> CriarPublicacao([FromBody] PublicacaoModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var publicacao = _mapper.Map<Publicacao>(model);

            var userId = _aspnetUser.GetUserId();

            publicacao.CriarPublicacao(Guid.Parse(userId));

            _context.Publicacoes.Add(publicacao);
            await _context.SaveChangesAsync();

            return Ok(publicacao.Id);
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> EditarPublicacao([FromRoute] Guid id,[FromBody] PublicacaoModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var publicacao = _mapper.Map<Publicacao>(model);

            var publicacaoDb = await _context.Publicacoes.FirstOrDefaultAsync(p => p.Id == id);
            publicacaoDb.Atualizar(publicacao);

            _context.Publicacoes.Update(publicacaoDb);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> ExcluirPublicacao([FromRoute] Guid id)
        {
            var publicacao = await _context.Publicacoes.FirstOrDefaultAsync(p => p.Id == id);
            if (publicacao == null)
            {
                return NotFound();
            }

            _context.Publicacoes.Remove(publicacao);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id}/comentario")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> AdicionarComentario([FromRoute] Guid id, [FromBody] ComentarioModel model)
        {
            if (string.IsNullOrEmpty(model.Descricao))
                return BadRequest("Descrição do comentário é obrigatória!");

            var userId = _aspnetUser.GetUserId();

            var comentario = new Comentario(model.Descricao, Guid.Parse(userId), id);

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return Ok(comentario.Id);
        }

        [HttpPut("{id}/comentario/{comentarioId}")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> EditarComentario([FromRoute] Guid id, Guid comentarioId, [FromBody] ComentarioModel model)
        {
            if (string.IsNullOrEmpty(model.Descricao))
                return BadRequest("Descrição do comentário é obrigatória!");

            var comentario = await _context.Comentarios.FirstOrDefaultAsync(p => p.Id == comentarioId);

            if (comentario == null)
            {
                return NotFound();
            }

            comentario.Atualizar(model.Descricao);

            _context.Comentarios.Update(comentario);
            await _context.SaveChangesAsync();

            return Ok(comentario.Id);
        }

        [HttpDelete("{id}/comentario/{comentarioId}")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> ExcluirComentario([FromRoute] Guid id, Guid comentarioId)
        {
            var comentario = await _context.Comentarios.FirstOrDefaultAsync(p => p.Id == comentarioId);

            if (comentario == null)
            {
                return NotFound();
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
