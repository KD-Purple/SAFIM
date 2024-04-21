using System;
using System.Collections.Generic;

namespace SAFIM.Models;

public partial class Alumno
{
    public int Matricula { get; set; }

    public string ApellidoP { get; set; } = null!;

    public string ApellidoM { get; set; } = null!;

    public string NombreAlumno { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Carrera { get; set; } = null!;

    public int Semestre { get; set; }

    public virtual Asesor? Asesor { get; set; }

    public virtual ICollection<Detallereporte> Detallereporte { get; set; } = new List<Detallereporte>();

    public virtual ICollection<Reporte> Reporte { get; set; } = new List<Reporte>();
}
