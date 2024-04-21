using System;
using System.Collections.Generic;

namespace SAFIM.Models;

public partial class Administrador
{
    public int Matricula { get; set; }

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public string NombreAdmin { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contraseña { get; set; } = null!;
}
