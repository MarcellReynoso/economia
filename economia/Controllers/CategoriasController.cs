using economia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace economia.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly EconomiaContext _context;

        public CategoriasController(EconomiaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var categorias = await _context.Categorias
                .Include(c => c.Tipo)
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();

            return View(categorias);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TipoId = new SelectList(_context.Tipos, "TipoId", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            categoria.UsuarioId = usuarioId;

            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                TempData["MensajeCategoria"] = "Categoría registrada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TipoId = new SelectList(_context.Tipos, "TipoId", "Nombre", categoria.TipoId);
            return View(categoria);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var categoria = await _context.Categorias
                .Where(c => c.CategoriaId == id && c.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (categoria == null) return NotFound();

            ViewBag.TipoId = new SelectList(_context.Tipos, "TipoId", "Nombre", categoria.TipoId);
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId) return NotFound();

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var categoriaExistente = await _context.Categorias
                .Where(c => c.CategoriaId == id && c.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (categoriaExistente == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    categoriaExistente.Nombre = categoria.Nombre;
                    categoriaExistente.Descripcion = categoria.Descripcion;
                    categoriaExistente.TipoId = categoria.TipoId;

                    _context.Update(categoriaExistente);
                    await _context.SaveChangesAsync();
                    TempData["MensajeCategoría"] = "Categoría actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Error de concurrencia.");
                }
            }

            ViewBag.TipoId = new SelectList(_context.Tipos, "TipoId", "Nombre", categoria.TipoId);
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var categoria = await _context.Categorias
                .Where(c => c.CategoriaId == id && c.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                TempData["MensajeCategoría"] = "Categoría eliminada.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
