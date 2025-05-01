using System;
using System.Collections.Generic;

namespace economia.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; }

    public string SegundoNombre { get; set; }

    public string ApellidoPaterno { get; set; }

    public string ApellidoMaterno { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Username { get; set; }

    public bool Activo { get; set; }

    public int RolId { get; set; }

    public virtual ICollection<Categoria> Categoria { get; set; } = new List<Categoria>();

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual Rol Rol { get; set; }
}
