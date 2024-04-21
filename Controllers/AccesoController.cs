using Microsoft.AspNetCore.Mvc;
using SAFIM.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SAFIM.Controllers
{
    public class AccesoController : Controller
    {
        private readonly SistemaAsesoriasContext _contexto;
        private readonly IHttpContextAccessor _acceso;

        public AccesoController(SistemaAsesoriasContext contexto, IHttpContextAccessor accesor)
        {
            _contexto = contexto;
            _acceso = accesor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string matricula, string contraseña)
        {
            var usuarioController = new UsuarioController(_contexto);
            bool login = usuarioController.EncontrarUsuario(matricula, contraseña);

            if (login)
            {
                string rol = usuarioController.ObtenerRol(matricula);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,matricula),
                    new Claim(ClaimTypes.Role,rol)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await _acceso.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                _acceso.HttpContext.Response.Cookies.Append("_Matricula", matricula); // Guardar la matrícula en la sesión

                switch (rol)
                {
                    case "Administrador":
                        return RedirectToAction("Index", "Administrador");
                    case "Asesor":
                        return RedirectToAction("Index", "Asesor");
                }
            }

            ViewBag.ErrorMessage = "Matricula y/o contraseña son incorrectas";
            return View("Index");
        }

        [Authorize]
        public IActionResult Logout()
        {
            _acceso.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //_acceso.HttpContext.Session.Clear(); // Limpiar la sesión al cerrar sesión
            //HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Acceso");
        }
    }
}



