using System;
using System.Collections.Generic;

namespace economia.Models;

public partial class Metodo
{
    public int MetodoId { get; set; }

    public string Nombre { get; set; }

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}
