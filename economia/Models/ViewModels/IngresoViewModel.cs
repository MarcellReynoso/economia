using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace economia.Models.ViewModels
{
    public class IngresoViewModel
    {
        public int GastoId { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría *")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        [Display(Name = "Método de Pago *")]
        public int MetodoId { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Display(Name = "Monto (S/.) *")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }
        
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public IEnumerable<SelectListItem>? Usuarios { get; set; }
        public IEnumerable<SelectListItem>? Categorias { get; set; }
        public IEnumerable<SelectListItem>? Metodos { get; set; }
    }
}
