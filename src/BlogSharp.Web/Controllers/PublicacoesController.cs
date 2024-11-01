using AutoMapper;
using BlogSharp.Data.Data;
using BlogSharp.Data.Entities;
using BlogSharp.Data.Interfaces;
using BlogSharp.Data.Models;
using BlogSharp.Web.Models;
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

        public async Task<IActionResult> Index(bool filtroPorUsuario = false)
        {
            var userId = _aspnetUser.GetUserId();
            var publicacoes = await _context.Publicacoes.Include(p => p.Autor).OrderByDescending(p => p.DataPublicacao).Where(p => !filtroPorUsuario || p.AutorId == Guid.Parse(userId)).ToListAsync();

            var resultado = _mapper.Map<List<PublicacaoResponseModel>>(publicacoes);

            ViewBag.FiltroPorUsuario = filtroPorUsuario;

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

        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = await _context.Publicacoes.FirstOrDefaultAsync(p => p.Id == id);
            var resultado = _mapper.Map<PublicacaoModel>(post);

            return View(resultado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(PublicacaoModel model)
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

            var publicacaoDb = await _context.Publicacoes.FirstOrDefaultAsync(p => p.Id == model.Id);
            publicacaoDb.Atualizar(publicacao);

            _context.Publicacoes.Update(publicacaoDb);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Excluir(Guid publicacaoId)
        {
            if (publicacaoId == null)
            {
                ModelState.AddModelError("", "ID da publicação inválido.");
                return RedirectToAction(nameof(Index));
            }

            var publicacao = await _context.Publicacoes.FirstOrDefaultAsync(p => p.Id == publicacaoId);
            if (publicacao == null)
            {
                return NotFound();
            }

            _context.Publicacoes.Remove(publicacao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var post = await _context.Publicacoes
                                            .Include(p => p.Autor)
                                            .Include(p => p.Comentarios.OrderByDescending(x => x.DataPublicacao))
                                                .ThenInclude(p => p.Autor)
                                            .FirstOrDefaultAsync(p => p.Id == id);

            var publicacao = _mapper.Map<PublicacaoResponseModel>(post);

            var viewModel = new PublicacaoComentarioViewModel()
            {
                Publicacao = publicacao,
                Comentario = new ComentarioModel(),
                PublicacaoId = id
            };

            var usuarioLogado = false;
            var usuarioAdmin = false;

            var userId = _aspnetUser.GetUserId();

            if (string.IsNullOrEmpty(userId))
                return View(viewModel);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(p => p.Id == Guid.Parse(userId));

            if (usuario != null && usuario.Administrador)
                usuarioAdmin = true;

            ViewBag.UsuarioId = Guid.Parse(userId);
            ViewBag.UsuarioAdmin = usuarioAdmin;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AdicionarComentario(PublicacaoComentarioViewModel model)
        {
            var userId = _aspnetUser.GetUserId();

            var comentario = new Comentario(model.Comentario.Descricao, Guid.Parse(userId), model.PublicacaoId);

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = model.PublicacaoId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditarComentario(Guid comentarioId, Guid publicacaoId, string descricao)
        {
            var comentario = await _context.Comentarios.FirstOrDefaultAsync(p => p.Id == comentarioId);

            if (comentario == null)
            {
                return NotFound();
            }

            comentario.Atualizar(descricao);

            _context.Comentarios.Update(comentario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = publicacaoId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ExcluirComentario(Guid comentarioId, Guid publicacaoId)
        {
            if (comentarioId == null)
            {
                ModelState.AddModelError("", "ID do comentário inválido.");
                return RedirectToAction("Details", new { id = publicacaoId });
            }

            var comentario = await _context.Comentarios.FirstOrDefaultAsync(p => p.Id == comentarioId);
            if (comentario == null)
            {
                return NotFound();
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = publicacaoId });
        }
    }
}
