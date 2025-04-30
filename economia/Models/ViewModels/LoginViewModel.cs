using System.ComponentModel.DataAnnotations;

namespace economia.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingresa un usuario.")]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Ingresa una contraseña.")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}
