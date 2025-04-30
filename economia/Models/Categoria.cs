using System;
using System.Collections.Generic;

namespace economia.Models;

public partial class Categoria
{
    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}
