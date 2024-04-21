using System;
using System.Collections.Generic;

namespace SAFIM.Models;

public partial class Asesor
{
    public int Matricula { get; set; }

    public string Contraseña { get; set; } = null!;

    public virtual ICollection<Asesoria> Asesoria { get; set; } = new List<Asesoria>();

    public virtual Alumno MatriculaNavigation { get; set; } = null!;
}
