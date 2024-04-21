using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAFIM.Models;
using System.Data;

namespace SAFIM.Controllers
{
    public class AsesoriaController : Controller
    {
        private readonly SistemaAsesoriasContext _contexto;
        private readonly IHttpContextAccessor _acceso;
        private readonly int? matricula;

        public AsesoriaController(SistemaAsesoriasContext contexto, IHttpContextAccessor accesor)
        {
            _contexto = contexto;
            _acceso = accesor;
            matricula = int.Parse(_acceso.HttpContext.Request.Cookies["_Matricula"]);
        }

        [Authorize(Roles = "Asesor")]
        [Route("asesor/asesorias")]
        public async Task<IActionResult> AsesoriasAsesor()
        {
            var asesorias = await _contexto.Asesoria
                .Include(a => a.ClaveMateriaNavigation)
                .Include(a => a.MatriculaAsesorNavigation)
                .Where(a => a.MatriculaAsesor == matricula).ToListAsync();
            return View("../Asesor/Asesorias/Index", asesorias);
        }

        [Authorize(Roles = "Administrador")]
        [Route("administrador/asesorias")]
        public async Task<IActionResult> Asesorias()
        {
            var asesorias = await _contexto.Asesoria
                .Include(a => a.ClaveMateriaNavigation)
                .Include(a => a.MatriculaAsesorNavigation)
                .ThenInclude(a => a.MatriculaNavigation).ToListAsync();

            return View("../Administrador/Asesorias/Index", asesorias);
        }

        [Authorize(Roles = "Administrador")]
        [Route("administrador/asesorias/registro")]
        public IActionResult RegistroAsesoria()
        {

            return View("../Administrador/Asesorias/Registro");
        }


        [Authorize(Roles = "Administrador")]
        [Route("administrador/asesorias/modificacion")]
        public async Task<IActionResult> ModificarAsesoria(int idAsesoria)
        {
            if (idAsesoria <= 0)
            {
                return RedirectToAction("Asesorias");
            }

            var asesoria = await _contexto.Asesoria
                .Include(a => a.ClaveMateriaNavigation)
                .Include(a => a.MatriculaAsesorNavigation)
                .ThenInclude(a => a.MatriculaNavigation)
                .Where(a => a.IdAsesoria.Equals(idAsesoria))
                .FirstOrDefaultAsync();

            return View("../Administrador/Asesorias/Modificar", asesoria);
        }



        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult NuevaAsesoria(int clavemateria, int matricula,
            string lunesinicio, string lunesfin,
            string martesinicio, string martesfin,
            string miercolesinicio, string miercolesfin,
            string juevesinicio, string juevesfin,
            string viernesinicio, string viernesfin)
        {

            String? horarioLunes = (String.IsNullOrEmpty(lunesinicio) && String.IsNullOrEmpty(lunesfin)) ? null : lunesinicio + " a " + lunesfin;
            String? horarioMartes = (String.IsNullOrEmpty(martesinicio) && String.IsNullOrEmpty(martesfin)) ? null : martesinicio + " a " + martesfin;
            String? horarioLMiercoles = (String.IsNullOrEmpty(miercolesinicio) && String.IsNullOrEmpty(miercolesfin)) ? null : miercolesinicio + " a " + miercolesfin;
            String? horarioJueves = (String.IsNullOrEmpty(juevesinicio) && String.IsNullOrEmpty(juevesfin)) ? null : juevesinicio + " a " + juevesfin;
            String? horarioViernes = (String.IsNullOrEmpty(viernesinicio) && String.IsNullOrEmpty(viernesfin)) ? null : viernesinicio + " a " + viernesfin;

            _contexto.Add(new Asesoria
            {
                MatriculaAsesor = matricula,
                ClaveMateria = clavemateria,
                HorarioLunes = horarioLunes,
                HorarioMartes = horarioMartes,
                HorarioMiercoles = horarioLMiercoles,
                HorarioJueves = horarioJueves,
                HorarioViernes = horarioViernes
            }
            );
            _contexto.SaveChanges();


            return RedirectToAction("Asesorias");

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult ActualizarAsesoria(int idAsesoria, int clavemateria, int matricula,
            string lunesinicio, string lunesfin,
            string martesinicio, string martesfin,
            string miercolesinicio, string miercolesfin,
            string juevesinicio, string juevesfin,
            string viernesinicio, string viernesfin)
        {

            String? horarioLunes = (String.IsNullOrEmpty(lunesinicio) && String.IsNullOrEmpty(lunesfin)) ? null : lunesinicio + " a " + lunesfin;
            String? horarioMartes = (String.IsNullOrEmpty(martesinicio) && String.IsNullOrEmpty(martesfin)) ? null : martesinicio + " a " + martesfin;
            String? horarioLMiercoles = (String.IsNullOrEmpty(miercolesinicio) && String.IsNullOrEmpty(miercolesfin)) ? null : miercolesinicio + " a " + miercolesfin;
            String? horarioJueves = (String.IsNullOrEmpty(juevesinicio) && String.IsNullOrEmpty(juevesfin)) ? null : juevesinicio + " a " + juevesfin;
            String? horarioViernes = (String.IsNullOrEmpty(viernesinicio) && String.IsNullOrEmpty(viernesfin)) ? null : viernesinicio + " a " + viernesfin;

            var asesoria = _contexto.Asesoria.Single(a => a.IdAsesoria.Equals(idAsesoria));

            asesoria.MatriculaAsesor = matricula;
            asesoria.ClaveMateria = clavemateria;
            asesoria.HorarioLunes = horarioLunes;
            asesoria.HorarioMartes = horarioMartes;
            asesoria.HorarioMiercoles = horarioLMiercoles;
            asesoria.HorarioJueves = horarioJueves;
            asesoria.HorarioViernes = horarioViernes;


            _contexto.SaveChanges();
            return RedirectToAction("Asesorias");
        }


        [Authorize(Roles = "Administrador")]
        [Route("administrador/asesorias/borrar")]
        public async Task<IActionResult> BorrarAsesoria(int idAsesoria)
        {
            if (idAsesoria <= 0)
            {
                return RedirectToAction("Asesorias");
            }

            var asesoria = await _contexto.Asesoria
                .Include(a => a.ClaveMateriaNavigation)
                .Include(a => a.MatriculaAsesorNavigation)
                .ThenInclude(a => a.MatriculaNavigation)
                .Where(a => a.IdAsesoria.Equals(idAsesoria))
                .FirstOrDefaultAsync();



            return View("../Administrador/Asesorias/BorrarAsesoria", asesoria);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Borrar(int idAsesoria)
        {
            var asesoria = _contexto.Asesoria.Find(idAsesoria);
            if (asesoria == null)
            {
                return View();
            }

            _contexto.Asesoria.Remove(asesoria);
            _contexto.SaveChanges();
            return RedirectToAction("Asesorias");
        }

        public int ContarAsesoriasAsesor(int matricula)
        {
            int asesorias = _contexto.Asesoria
                .Where(a => a.MatriculaAsesor == matricula).Count();
            return asesorias;
        }
        //       public ActionResult ObtenerClavesMaterias()
        //     {
        //       var clavesMaterias = _contexto.Materia.ToList(); // Suponiendo que 'db' es tu contexto de base de datos

        //     var data = clavesMaterias.Select(m => new
        //   {
        //     ClaveMateria = m.ClaveMateria,
        //   NombreMateria = m.NombreMateria
        //});

        //return Json(data);
        //}
        public IActionResult ObtenerClavesMaterias()
        {
            var clavesMaterias = _contexto.Materia.Select(m => new
            {
                ClaveMateria = m.ClaveMateria,
                NombreMateria = m.NombreMateria
            }).ToList();

            return Json(clavesMaterias);
        }


    }
}
