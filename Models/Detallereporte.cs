using System;
using System.Collections.Generic;

namespace SAFIM.Models;

public partial class Detallereporte
{
    public int IdDetalle { get; set; }

    public int IdReporte { get; set; }

    public int? MatriculaAlumno { get; set; }

    public int? Grupo { get; set; }

    public string? ProgramaEducativo { get; set; }

    public string? Temas { get; set; }

    public string? Tiempo { get; set; }

    public string? Comentarios { get; set; }

    public virtual Reporte IdReporteNavigation { get; set; } = null!;

    public virtual Alumno? MatriculaAlumnoNavigation { get; set; }
}
