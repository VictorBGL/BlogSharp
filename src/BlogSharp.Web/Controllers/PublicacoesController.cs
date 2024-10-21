using AutoMapper;
using BlogSharp.Data.Data;
using BlogSharp.Data.Entities;
using BlogSharp.Data.Interfaces;
using BlogSharp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSharp.Web.Controllers
{
    public class PublicacoesController : Controller
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAspnetUser _aspnetUser;

        public PublicacoesController(
            ApiDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, IAspnetUser aspnetUser)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _aspnetUser = aspnetUser;
        }

        public async Task<IActionResult> Index()
        {
            var publicacoes = await _context.Publicacoes.Include(p => p.Autor).OrderByDescending(p => p.DataPublicacao).ToListAsync();

            var resultado = _mapper.Map<List<PublicacaoResponseModel>>(publicacoes);

            return View(resultado);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(PublicacaoModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ImagemFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imagens");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagemFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImagemFile.CopyToAsync(fileStream);
                }

                model.Imagem = uniqueFileName;
            }

            var publicacao = _mapper.Map<Publicacao>(model);

            var userId = _aspnetUser.GetUserId();

            publicacao.CriarPublicacao(Guid.Parse(userId));

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
