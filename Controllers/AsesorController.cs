using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SAFIM.Models;
using System.Data;
using System.Linq;

namespace SAFIM.Controllers
{
    public class AsesorController : Controller
    {

        private readonly SistemaAsesoriasContext _contexto;
        private readonly IHttpContextAccessor _acceso;

        public AsesorController(SistemaAsesoriasContext contexto, IHttpContextAccessor accesor)
        {
            _contexto = contexto;
            _acceso = accesor;
        }

        [Authorize(Roles = "Asesor")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Administrador")]
        [Route("administrador/asesores")]
        public async Task<IActionResult> Asesores(string ordenar, string buscar, string tipo)
        {
            ViewData["OrdenarMatricula"] = String.IsNullOrEmpty(ordenar) ? "matricula_desc" : "";
            ViewData["OrdenNombre"] = ordenar == "nombre" ? "nombre_desc" : "nombre";
            ViewData["Buscador"] = buscar;

            var asesores = _contexto.Asesor
                .Include(a => a.MatriculaNavigation).AsQueryable();

            if (!buscar.IsNullOrEmpty())
            {
                switch (tipo)
                {
                    case "matricula":
                        asesores = asesores.Where(a => a.Matricula.ToString().Contains(buscar));
                        break;
                    case "nombre":
                        asesores = asesores.Where(a => (a.MatriculaNavigation.NombreAlumno + " " + a.MatriculaNavigation.ApellidoP + " " + a.MatriculaNavigation.ApellidoM).Contains(buscar));
                        break;

                }
            }

            switch (ordenar)
            {
                case "matricula_desc":
                    asesores.OrderByDescending(a => a.Matricula);
                    break;
                case "nombre":
                    asesores.OrderBy(a => a.MatriculaNavigation.NombreAlumno);
                    break;
                case "nombre_desc":
                    asesores.OrderByDescending(a => a.MatriculaNavigation.NombreAlumno);
                    break;
                default:
                    asesores.OrderBy(a => a.Matricula);
                    break;
            }

            return View("../Administrador/Asesores/Index", await asesores.ToListAsync());
        }

        [Authorize(Roles = "Administrador")]
        [Route("administrador/asesores/registro")]
        public IActionResult RegistroAsesor()
        {
            ViewBag.Error = TempData["error"];
            return View("../Administrador/Asesores/Registro");
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult NuevoAsesor(int matricula)
        {
            try
            {
                int semestre = _contexto.Alumno.Where(a => a.Matricula.Equals(matricula)).Select(a => a.Semestre).FirstOrDefault();

                if (semestre < 2)
                {
                    TempData["error"] = "Un alumno de primer semestre no puede ser asesor.";
                    return RedirectToAction("RegistroAsesor");
                }

                _contexto.Add(new Asesor
                {
                    Matricula = matricula,
                    Contraseña = "AL" + matricula
                }
                );
                _contexto.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                int? numero = (e.InnerException as SqlException)?.Number;

                switch (numero)
                {
                    case 2627:
                        TempData["error"] = "El asesor ya esta registrado en el sistema";
                        break;
                    case 547:
                        TempData["error"] = "Ingrese una matricula valida";
                        break;
                }

                return RedirectToAction("RegistroAsesor");
            }

            _contexto.Add(new Usuario
            {
                Matricula = matricula.ToString(),
                IdRol = 2
            });
            _contexto.SaveChanges();

            return RedirectToAction("Asesores");

        }


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> DetallesAsesor(int matricula)
        {

            if (matricula <= 0)
            {
                return RedirectToAction("Asesores");
            }

            var asesor = await _contexto.Asesor
                .Include(a => a.MatriculaNavigation)
                .Where(a => a.Matricula.Equals(matricula))
                .FirstOrDefaultAsync();

            ViewBag.numAsesorias = new AsesoriaController(_contexto, _acceso).ContarAsesoriasAsesor(matricula);

            return View("../Administrador/Asesores/DetallesAsesor", asesor);
        }


        [Authorize(Roles = "Administrador")]
        [Route("administrador/asesores/borrar")]
        public async Task<IActionResult> ConfirmarBorrarAsesor(int matricula)
        {
            if (matricula <= 0)
            {
                return RedirectToAction("Asesores");
            }

            var asesor = await _contexto.Asesor
                .Include(a => a.MatriculaNavigation)
                .Where(a => a.Matricula.Equals(matricula))
                .FirstOrDefaultAsync();



            return View("../Administrador/Asesores/BorrarAsesor", asesor);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult BorrarAsesor(int matricula)
        {

            var asesor = _contexto.Asesor.Find(matricula);
            var usuario = _contexto.Usuario.Find(matricula.ToString());
            if (asesor == null || usuario == null)
            {
                return View();
            }

            _contexto.Asesor.Remove(asesor);
            _contexto.Usuario.Remove(usuario);
            _contexto.SaveChanges();

            return RedirectToAction("Asesores");
        }

        [Route("asesor/obtenerasesores")]
        [HttpPost]
        public async Task<JsonResult> ObtenerAsesor(string matricula)
        {
            /*Query para retornar una lista de los alumnos en base a la matricula
            En el Select se nombro la matricula como label,
            ya que label es requerida para el autocompletado en la lista desplegable
            del autocompletado en ajax, sin este 'label', no se mostrarían las matrículas 
            en la vista del Registro de un Asesor.
            */
            var alumno = await _contexto.Asesor
                .Include(a => a.MatriculaNavigation)
                .Where(a => a.Matricula.ToString().Contains(matricula))
                .Select(a => new
                {
                    label = a.Matricula,
                    nombre = a.MatriculaNavigation.NombreAlumno + " " + a.MatriculaNavigation.ApellidoP + " " + a.MatriculaNavigation.ApellidoM,
                    correo = a.MatriculaNavigation.Correo,
                    carrera = a.MatriculaNavigation.Carrera,
                    semestre = a.MatriculaNavigation.Semestre,
                })
                .Take(5)
                .ToListAsync();

            return Json(alumno);


        }


        [Route("asesor/obteneralumnos")]
        [HttpPost]
        public async Task<JsonResult> ObtenerAlumno(string matricula)
        {
            /*Query para retornar una lista de los alumnos en base a la matricula
            En el Select se nombro la matricula como label,
            ya que label es requerida para el autocompletado en la lista desplegable
            del autocompletado en ajax, sin este 'label', no se mostrarían las matrículas 
            en la vista del Registro de un Asesor.
            */
            var alumno = await _contexto.Alumno
                .Include(a => a.Asesor)
                .Where(a => a.Matricula.ToString().Contains(matricula))
                .Where(a => a.Semestre > 1)
                .Where(a => a.Matricula != a.Asesor.Matricula)
                .Select(a => new
                {
                    label = a.Matricula,
                    nombre = a.NombreAlumno + " " + a.ApellidoP + " " + a.ApellidoM,
                    correo = a.Correo,
                    carrera = a.Carrera,
                    semestre = a.Semestre,
                })
                .Take(5)
                .ToListAsync();

            return Json(alumno);


        }

        [Route("asesor/obteneralumnos/todos")]
        [HttpPost]
        public async Task<JsonResult> ObtenerAlumnoTodo(string matricula)
        {
            /*Query para retornar una lista de los alumnos en base a la matricula
            En el Select se nombro la matricula como label,
            ya que label es requerida para el autocompletado en la lista desplegable
            del autocompletado en ajax, sin este 'label', no se mostrarían las matrículas 
            en la vista del Registro de un Asesor.
            */
            var alumno = await _contexto.Alumno
                .Where(a => a.Matricula.ToString().Contains(matricula))
                .Select(a => new
                {
                    label = a.Matricula,
                    nombre = a.NombreAlumno + " " + a.ApellidoP + " " + a.ApellidoM,
                    correo = a.Correo,
                    carrera = a.Carrera,
                    semestre = a.Semestre,
                })
                .Take(5)
                .ToListAsync();

            return Json(alumno);
        }
    }
}