using economia.Models.ViewModels;
using economia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace economia.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly EconomiaContext _context;

        public UsuariosController(EconomiaContext context )
        {
            _context = context;
        }

        [Authorize (Roles = "Administrador")]
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
                    Activo = true
                };

                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
                Console.WriteLine("Usuario registrado correctamente");
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
