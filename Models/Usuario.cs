using System;
using System.Collections.Generic;

namespace SAFIM.Models;

public partial class Usuario
{
    public string Matricula { get; set; } = null!;

    public int? IdRol { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
