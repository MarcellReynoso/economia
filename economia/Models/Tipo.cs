using System;
using System.Collections.Generic;

namespace economia.Models;

public partial class Tipo
{
    public int TipoId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}
