using economia.Models.ViewModels;
using economia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace economia.Controllers
{
    [Authorize]
    public class GastosController : Controller
    {
        private readonly EconomiaContext _context;

        public GastosController(EconomiaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Obtener el ID del usuario logueado
            var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var controlGastosContext = _context.Gastos
                .Where(g => g.UsuarioId == usuarioId)
                .Include(g => g.Categoria)
                .Include(g => g.Metodo)
                .Include(g => g.Usuario)
                .Include(g => g.Tipo);
            return View(await controlGastosContext.ToListAsync());
        }

        public async Task<IActionResult> Detalles()
        {
            ViewBag.Usuarios = await _context.Usuarios.ToListAsync();
            ViewBag.Categorias = await _context.Categorias.ToListAsync();
            ViewBag.Metodos = await _context.Metodos.ToListAsync();
            ViewBag.Tipos = await _context.Tipos.ToListAsync();

            // Obtener el ID del usuario logueado
            var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var gastos = await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId)
                .Include(g => g.Usuario)
                .Include(g => g.Categoria)
                .Include(g => g.Metodo)
                .Include(g => g.Tipo)
                .ToListAsync();

            ViewBag.TotalGastos = await _context.Gastos
                .Where(g => g.TipoId == 2 && g.UsuarioId == usuarioId)
                .SumAsync(g => g.Monto);

            ViewBag.TotalIngresos = await _context.Gastos
                .Where(g => g.TipoId == 1 && g.UsuarioId == usuarioId)
                .SumAsync(g => g.Monto);

            ViewBag.Total = ViewBag.TotalIngresos - ViewBag.TotalGastos;
            if (ViewBag.Total < 0)
            {
                ViewBag.Total = 0;
            }

            return View(gastos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            ViewBag.CategoriaId = new SelectList(_context.Categorias
                .Where(c => c.TipoId == 2 && c.UsuarioId == usuarioId), "CategoriaId", "Nombre");
            ViewBag.MetodoId = new SelectList(_context.Metodos, "MetodoId", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GastoViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Obtener el ID del usuario logueado
                var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var gasto = new Gasto
                {
                    UsuarioId = userId,
                    CategoriaId = model.CategoriaId,
                    MetodoId = model.MetodoId,
                    TipoId = 2,
                    Monto = model.Monto,
                    Descripcion = model.Descripcion,
                    Fecha = model.Fecha
                };
                _context.Add(gasto);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Movimiento registrado correctamente.";
                return RedirectToAction("Detalles", "Gastos");
            }

            // Si algo falla, recargamos las listas de nuevo
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "CategoriaId", "Nombre", model.CategoriaId);
            ViewBag.MetodoId = new SelectList(_context.Metodos, "MetodoId", "Nombre", model.MetodoId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var gasto = await _context.Gastos
                .Where(g => g.GastoId == id && g.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (gasto == null)
                return NotFound();

            var model = new GastoViewModel
            {
                GastoId = gasto.GastoId,
                CategoriaId = gasto.CategoriaId,
                MetodoId = gasto.MetodoId,
                TipoId = gasto.TipoId,
                Monto = gasto.Monto,
                Descripcion = gasto.Descripcion,
                Fecha = gasto.Fecha
            };

            ViewBag.CategoriaId = new SelectList(_context.Categorias, "CategoriaId", "Nombre", gasto.CategoriaId);
            ViewBag.MetodoId = new SelectList(_context.Metodos, "MetodoId", "Nombre", gasto.MetodoId);
            ViewBag.TipoId = new SelectList(_context.Tipos, "TipoId", "Nombre", gasto.TipoId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GastoViewModel gasto)
        {
            if (id != gasto.GastoId)
                return NotFound();

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var gastoExistente = await _context.Gastos
                .Where(g => g.GastoId == id && g.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (gastoExistente == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    gastoExistente.CategoriaId = gasto.CategoriaId;
                    gastoExistente.MetodoId = gasto.MetodoId;
                    gastoExistente.Monto = gasto.Monto;
                    gastoExistente.Descripcion = gasto.Descripcion;
                    gastoExistente.Fecha = gasto.Fecha;
                    gastoExistente.TipoId = gasto.TipoId;

                    TempData["Mensaje"] = "Movimiento editado correctamente.";
                    _context.Update(gastoExistente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Error de concurrencia");
                }

                return RedirectToAction(nameof(Detalles));
            }

            ViewBag.CategoriaId = new SelectList(_context.Categorias, "CategoriaId", "Nombre", gasto.CategoriaId);
            ViewBag.MetodoId = new SelectList(_context.Metodos, "MetodoId", "Nombre", gasto.MetodoId);
            ViewBag.TipoId = new SelectList(_context.Tipos, "TipoId", "Nombre", gasto.TipoId);

            return View(gasto);
        }

        [HttpGet]
        public IActionResult Ingreso()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            ViewBag.Categorias = new SelectList(_context.Categorias
                .Where(c => c.TipoId == 1 && c.UsuarioId == usuarioId), "CategoriaId", "Nombre");
            ViewBag.Metodos = new SelectList(_context.Metodos, "MetodoId", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ingreso(IngresoViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var ingreso = new Gasto
                {
                    UsuarioId = usuarioId,
                    CategoriaId = model.CategoriaId,
                    MetodoId = model.MetodoId,
                    TipoId = 1,
                    Monto = model.Monto,
                    Descripcion = model.Descripcion,
                    Fecha = model.Fecha
                };

                await _context.AddAsync(ingreso);
                await _context.SaveChangesAsync();
                TempData["MensajeIngreso"] = "Ingreso registrado correctamente.";
                return RedirectToAction("Detalles", "Gastos");
            }
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "CategoriaId", "Nombre", model.CategoriaId);
            ViewBag.MetodoId = new SelectList(_context.Metodos, "MetodoId", "Nombre", model.MetodoId);

            return View(model);
        }


        //Gráficos
        public async Task<IActionResult> Reportes()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var porCategoria = await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId && g.TipoId == 2)
                .Include(g => g.Categoria)
                .GroupBy(g => g.Categoria.Nombre)
                .Select(g => new GastoPorCategoriaViewModel
                {
                    Categoria = g.Key,
                    Total = g.Sum(x => x.Monto)
                })
                .ToListAsync();

            var porFecha = await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId && g.TipoId == 2)
                .GroupBy(g => g.Fecha.Date)
                .OrderBy(g => g.Key)
                .Select(g => new GastoPorFechaViewModel
                {
                    Fecha = g.Key,
                    Total = g.Sum(x => x.Monto)
                })
                .ToListAsync();

            var totalGastos = porFecha.Sum(x => x.Total);
            var diasConGasto = porFecha.Count();

            DashboardViewModel dashboardViewModel = new DashboardViewModel()
            {
                PorCategoria = porCategoria,
                PorFecha = porFecha,
                TotalGastos = totalGastos,
                PromedioDiario = diasConGasto > 0 ? totalGastos/diasConGasto : 0
            };

            return View(dashboardViewModel);
        }

    }
}
