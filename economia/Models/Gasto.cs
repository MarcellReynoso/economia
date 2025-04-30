using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace economia.Models;

public partial class Gasto
{
    public int GastoId { get; set; }

    public int UsuarioId { get; set; }

    public int CategoriaId { get; set; }

    public int MetodoId { get; set; }

    public decimal Monto { get; set; }

    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    public DateTime Fecha { get; set; }

    public int TipoId { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual Metodo Metodo { get; set; } = null!;

    public virtual Tipo Tipo { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;

}
