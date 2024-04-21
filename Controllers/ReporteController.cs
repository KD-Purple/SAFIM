using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAFIM.Models;
using System.Data;
using System.Security.Claims;

namespace SAFIM.Controllers
{
    public class ReporteController : Controller
    {

        private readonly SistemaAsesoriasContext _contexto;
        private readonly IHttpContextAccessor _acceso;
        private readonly int? matricula;

        public ReporteController(SistemaAsesoriasContext contexto, IHttpContextAccessor accesor)
        {
            _contexto = contexto;
            _acceso = accesor;
            matricula = int.Parse(_acceso.HttpContext.Request.Cookies["_Matricula"]);
        }

        [Authorize(Roles = "Administrador")]
        [Route("administrador/reportes")]
        public async Task<IActionResult> Reportes(string ordenar, string buscar, string tipo)
        {
            ViewData["OrdenIdAsesoria"] = String.IsNullOrEmpty(ordenar) ? "id_desc" : "";
            ViewData["OrdenNombre"] = ordenar == "nombre" ? "nombre_desc" : "nombre";
            ViewData["OrdenFecha"] = ordenar == "fecha" ? "fecha_desc" : "fecha";
            ViewData["Buscador"] = buscar;

            var reportes = _contexto.Reporte
                .Include(r => r.MatriculaAsesorNavigation)
                .Include(r => r.ClaveMateriaNavigation)
                .Include(r => r.Detallereporte)
                .AsQueryable();

            switch (tipo)
            {
                case "nombre":
                    reportes = reportes.Where(r => (r.MatriculaAsesorNavigation.NombreAlumno + " " + r.MatriculaAsesorNavigation.ApellidoP + " " + r.MatriculaAsesorNavigation.ApellidoM).Contains(buscar));
                    break;
                case "fecha":
                    reportes = reportes.Where(r => r.Fecha.Equals(buscar));
                    break;
            }

            switch (ordenar)
            {
                case "id_desc":
                    reportes = reportes.OrderByDescending(r => r.IdAsesoria);
                    break;
                case "nombre":
                    reportes = reportes.OrderBy(r => r.IdAsesoria);
                    break;
                case "nombre_desc":
                    reportes = reportes.OrderByDescending(r => r.IdAsesoria);
                    break;
                case "fecha":
                    reportes = reportes.OrderBy(r => r.Fecha);
                    break;
                case "fecha_desc":
                    reportes = reportes.OrderByDescending(r => r.Fecha);
                    break;
                default:
                    reportes = reportes.OrderBy(r => r.IdAsesoria);
                    break;
            }

            return View("../Administrador/Reportes/Index", await reportes.AsNoTracking().ToListAsync());
        }

        [Authorize(Roles = "Asesor")]
        [Route("asesor/reportes")]
        public async Task<IActionResult> ReportesAsesor()
        {
            var reportes = await _contexto.Reporte
                .Include(r => r.ClaveMateriaNavigation)
                .Include(r => r.Detallereporte)
                .Where(r => r.MatriculaAsesor.Equals(matricula))
                .ToListAsync();
            return View("../Asesor/Reportes/Index", reportes);
        }

        [Authorize(Roles = "Asesor")]
        public IActionResult RealizarReporte(int idReporte)
        {
            ViewBag.IdReporte = idReporte;

            return View("../Asesor/Reportes/Realizar");
        }

        [Authorize(Roles = "Asesor")]
        [HttpPost]
        public IActionResult NuevoReporte(int idReporte, List<int> matricula, List<int> grupo, List<string> carrera,
    List<string> temas, List<string> tiempo, List<string> comentario)
        {
            for (int i = 0; i < matricula.Count; i++)
            {
                switch (tiempo[i])
                {
                    case "tiempo1":
                        tiempo[i] = "10 - 30 min";
                        break;
                    case "tiempo2":
                        tiempo[i] = "30 - 45 min";
                        break;
                    case "tiempo3":
                        tiempo[i] = "45 - 60 min";
                        break;
                    case "tiempo4":
                        tiempo[i] = "+ 60 min";
                        break;
                }

                _contexto.Add(new Detallereporte
                {
                    IdReporte = idReporte,
                    MatriculaAlumno = matricula[i],
                    Grupo = grupo[i],
                    ProgramaEducativo = carrera[i],
                    Temas = temas[i],
                    Tiempo = tiempo[i],
                    Comentarios = comentario[i]
                });

                _contexto.SaveChanges();
            }

            return RedirectToAction("ReportesAsesor");
        }

        [Route("asesor/reportespendientes")]
        [HttpPost]
        public async Task<JsonResult> ReportesPendientes()
        {
            var reportespendientes = 0;
            var reportes = await _contexto.Reporte
                .Include(r => r.Detallereporte)
                .Where(r => r.MatriculaAsesor.Equals(matricula))
                .ToListAsync();

            foreach (var r in reportes)
            {
                if (r.Detallereporte.Count() == 0)
                {
                    reportespendientes++;
                }
            }
            return Json(reportespendientes);
        }

    }
}
