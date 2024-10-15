using AutoMapper;
using BlogSharp.Data.Data;
using BlogSharp.Data.Entities;
using BlogSharp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSharp.Web.Controllers
{
    public class PublicacoesController : Controller
    {
        private readonly ApiDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public PublicacoesController(
            ApiDbContext context, IHttpContextAccessor accessor, IMapper mapper)
        {
            _context = context;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var publicacoes = await _context.Publicacoes.Include(p => p.Autor).OrderBy(p => p.DataPublicacao).ToListAsync();

            var resultado = _mapper.Map<List<PublicacaoResponseModel>>(publicacoes);

            return View(resultado);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(PublicacaoModel model)
        {
            var publicacao = _mapper.Map<Publicacao>(model);

            publicacao.CriarPublicacao(Guid.Parse("E30048E7-B57F-428C-9251-D7FCC88B4669"));

            _context.Publicacoes.Add(publicacao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
