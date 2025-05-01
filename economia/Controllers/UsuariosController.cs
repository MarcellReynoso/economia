using economia.Models.ViewModels;
using economia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace economia.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly EconomiaContext _context;

        public UsuariosController(EconomiaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            ViewBag.Roles = new SelectList(_context.Roles.ToList(), "RolId", "Nombre");

            var usuarios = await _context.Usuarios
                .ToListAsync();

            return View(usuarios);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Registro()
        {
            ViewBag.Roles = new SelectList(_context.Roles.ToList(), "RolId", "Nombre");
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("Password", "Las contraseñas no coinciden");
                    ViewBag.Roles = new SelectList(_context.Roles.ToList(), "RolId", "Nombre");
                    return View(model);
                }

                var usuario = new Usuario()
                {
                    Nombre = model.Nombre,
                    SegundoNombre = model.SegundoNombre,
                    ApellidoPaterno = model.ApellidoPaterno,
                    ApellidoMaterno = model.ApellidoMaterno,
                    Email = model.Email,
                    Password = model.Password,
                    Username = model.Username,
                    RolId = model.RolId,
                    Activo = model.Activo
                };

                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
                TempData["MensajeUsuario"] = "Usuario registrado correctamente.";
                return RedirectToAction("Index", "Usuarios");
            }
            return View(model);
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioId == id);

            var model = new RegistroViewModel()
            {
                UsuarioId = usuario.UsuarioId,
                Nombre = usuario.Nombre,
                SegundoNombre = usuario.SegundoNombre,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno,
                Email = usuario.Email,
                Username = usuario.Username,
                RolId = usuario.RolId,
                Activo = usuario.Activo
            };
            ViewBag.Roles = new SelectList(_context.Roles.ToList(), "RolId", "Nombre");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, RegistroViewModel model)
        {
            if (id != model.UsuarioId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.UsuarioId == id);
                if (usuario == null)
                {
                    return NotFound();
                }
                usuario.Nombre = model.Nombre;
                usuario.SegundoNombre = model.SegundoNombre;
                usuario.ApellidoPaterno = model.ApellidoPaterno;
                usuario.ApellidoMaterno = model.ApellidoMaterno;
                usuario.Email = model.Email;
                usuario.Username = model.Username;
                usuario.Activo = model.Activo;
                usuario.RolId = model.RolId;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    if (model.Password != model.ConfirmPassword)
                    {
                        ModelState.AddModelError("Password", "Las contraseñas no coinciden.");
                        ViewBag.Roles = new SelectList(_context.Roles.ToList(), "RolId", "Nombre", model.RolId);
                        return View(model);
                    }
                    usuario.Password = model.Password;
                }
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                TempData["MensajeUsuario"] = "Usuario editado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Roles = new SelectList(_context.Roles.ToList(), "RolId", "Nombre");
            return View(model);
        }
    }
}
