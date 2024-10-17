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
            var publicacoes = await _context.Publicacoes.Include(p => p.Autor).OrderByDescending(p => p.DataPublicacao).ToListAsync();

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

            publicacao.CriarPublicacao(Guid.Parse("AB2A5542-C8AF-4861-B363-4EB94C296F52"));

            _context.Publicacoes.Add(publicacao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var post = await _context.Publicacoes.Include(p => p.Autor).Include(p => p.Comentarios.OrderByDescending(x => x.DataPublicacao)).FirstOrDefaultAsync(p => p.Id == id);

            var resultado = _mapper.Map<PublicacaoResponseModel>(post);

            return View(resultado);
        }
    }
}
