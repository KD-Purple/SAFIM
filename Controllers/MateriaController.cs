using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAFIM.Models;
using System.Data;

namespace SAFIM.Controllers
{
    public class MateriaController : Controller
    {

        private readonly SistemaAsesoriasContext _contexto;

        public MateriaController(SistemaAsesoriasContext contexto)
        {
            _contexto = contexto;
        }

        [Route("materias/obtenermaterias/clave")]
        [HttpPost]
        public async Task<JsonResult> ObtenerMateriaClave(string clavemateria)
        {
            /*Query para retornar una lista de las materias en base a la clave de la materia,
            en el Select se nombro la la clave y el nombre de la materia como label,
            ya que label es requerida para el autocompletado en la lista desplegable
            del autocompletado en ajax, sin este 'label', no se mostrarían las materias 
            en la vista del Registro o Modificacion de una Asesoria.
            */
            var materia = await _contexto.Materia.
           Where(a => a.ClaveMateria.ToString().Contains(clavemateria))
           .Select(a => new
           {
               label = a.ClaveMateria + " - " + a.NombreMateria,
               clave = a.ClaveMateria,
               materia = a.NombreMateria
           })
           .Take(10)
           .ToListAsync();

            return Json(materia);


        }

        [Route("materias/obtenermaterias/nombre")]
        [HttpPost]
        public async Task<JsonResult> ObtenerMateriaNombre(string nombremateria)
        {
            /*Query para retornar una lista de las materias en base al nombre,
            en el Select se nombro la la clave y el nombre de la materia como label,
            ya que label es requerida para el autocompletado en la lista desplegable
            del autocompletado en ajax, sin este 'label', no se mostrarían las materias 
            en la vista del Registro o Modificacion de una Asesoria.
            */
            var materia = await _contexto.Materia.
           Where(a => a.NombreMateria.Contains(nombremateria))
           .Select(a => new
           {
               label = a.ClaveMateria + " - " + a.NombreMateria,
               clave = a.ClaveMateria,
               materia = a.NombreMateria
           })
           .Take(10)
           .ToListAsync();

            return Json(materia);


        }

    }
}
