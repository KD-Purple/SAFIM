using System;
using System.Collections.Generic;

namespace SAFIM.Models;

public partial class Reporte
{
    public int IdReporte { get; set; }

    public int IdAsesoria { get; set; }

    public int MatriculaAsesor { get; set; }

    public int ClaveMateria { get; set; }

    public string? Horario { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Materia ClaveMateriaNavigation { get; set; } = null!;

    public virtual ICollection<Detallereporte> Detallereporte { get; set; } = new List<Detallereporte>();

    public virtual Alumno MatriculaAsesorNavigation { get; set; } = null!;
}
