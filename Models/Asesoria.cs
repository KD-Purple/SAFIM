using System;
using System.Collections.Generic;

namespace SAFIM.Models;

public partial class Asesoria
{
    public int IdAsesoria { get; set; }

    public int MatriculaAsesor { get; set; }

    public int ClaveMateria { get; set; }

    public string? HorarioLunes { get; set; }

    public string? HorarioMartes { get; set; }

    public string? HorarioMiercoles { get; set; }

    public string? HorarioJueves { get; set; }

    public string? HorarioViernes { get; set; }

    public virtual Materia ClaveMateriaNavigation { get; set; } = null!;

    public virtual Asesor MatriculaAsesorNavigation { get; set; } = null!;
}
