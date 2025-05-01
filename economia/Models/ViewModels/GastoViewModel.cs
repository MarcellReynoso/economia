using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace economia.Models.ViewModels
{
    public class GastoViewModel
    {
        public int GastoId { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Display(Name = "Monto*")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Display(Name = "Fecha*")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Método de Pago")]
        public int MetodoId { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Tipo de movimiento")]
        public int TipoId { get; set; }

        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }

        public IEnumerable<SelectListItem>? Usuarios { get; set; }
        public IEnumerable<SelectListItem>? Categorias { get; set; }
        public IEnumerable<SelectListItem>? Metodos { get; set; }
        public IEnumerable<SelectListItem>? Tipos { get; set; }
    }
}
