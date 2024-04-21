using System;
using System.Collections.Generic;

namespace SAFIM.Models;

public partial class Materia
{
    public int ClaveMateria { get; set; }

    public string NombreMateria { get; set; } = null!;

    public virtual ICollection<Asesoria> Asesoria { get; set; } = new List<Asesoria>();

    public virtual ICollection<Reporte> Reporte { get; set; } = new List<Reporte>();
}
