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

    public string Descripcion { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
    public DateTime Fecha { get; set; }

    public int TipoId { get; set; }

    public virtual Categoria Categoria { get; set; }

    public virtual Metodo Metodo { get; set; }

    public virtual Tipo Tipo { get; set; }

    public virtual Usuario Usuario { get; set; }
}
