using System.ComponentModel.DataAnnotations;

namespace economia.Models.ViewModels
{
    public class RegistroViewModel
    {
        public string? Nombre { get; set; }

        [Display(Name = "Segundo nombre")]
        public string? SegundoNombre { get; set; }

        [Display(Name = "Apellido paterno")]
        public string? ApellidoPaterno { get; set; }

        [Display(Name = "Apellido materno")]
        public string? ApellidoMaterno { get; set; }

        public string? Email { get; set; }

        [Display(Name = "Contraseña")]
        public string Password { get; set; } = null!;

        [Display(Name = "Confirmar contraseña")]
        public string ConfirmPassword { get; set; } = null!;

        public string Username { get; set; } = null!;

        public int RolId { get; set; }
    }
}
