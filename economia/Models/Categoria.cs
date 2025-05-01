using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace economia.Models;

public partial class Categoria
{
    public int CategoriaId { get; set; }

    [Display(Name = "Nombre*")]
    public string Nombre { get; set; }

    [Display(Name = "Descripción")]
    public string Descripcion { get; set; }

    public int UsuarioId { get; set; }

    [Display(Name = "Tipo de movimiento*")]
    public int TipoId { get; set; }

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual Tipo Tipo { get; set; }

    public virtual Usuario Usuario { get; set; }
}
