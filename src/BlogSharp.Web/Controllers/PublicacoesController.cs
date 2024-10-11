using Microsoft.AspNetCore.Mvc;

namespace BlogSharp.Web.Controllers
{
    public class PublicacoesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("{id}")]
        //public IActionResult Details(Guid id)
        //{
        //    return View();
        //}

        //[HttpGet("nova-publicacao")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost("nova-publicacao")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //[HttpGet("editar/{id}")]
        //public IActionResult Edit(Guid id)
        //{
        //    return View();
        //}

        //[HttpPost("editar/{id}")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Guid id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //[HttpGet("excluir/{id}")]
        //public IActionResult Delete(Guid id)
        //{
        //    return View();
        //}

        //[HttpPost("excluir/{id}")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(Guid id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
