using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SAFIM.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace SAFIM.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly SistemaAsesoriasContext _contexto;

        public UsuarioController(SistemaAsesoriasContext contexto)
        {
            _contexto = contexto;
        }

        public bool EncontrarUsuario(string matricula, string contraseña)
        {
            var usuario = _contexto.Usuario.FirstOrDefault(u => u.Matricula == matricula);

            if (usuario != null)
            {
                if (usuario.IdRol == 1)
                {
                    var administrador = _contexto.Administrador.FirstOrDefault(a => a.Matricula == int.Parse(matricula) && a.Contraseña == contraseña);

                    if (administrador != null)
                    {
                        return true;
                    }
                }
                else if (usuario.IdRol == 2)
                {
                    var asesor = _contexto.Asesor.FirstOrDefault(a => a.Matricula == int.Parse(matricula) && a.Contraseña == contraseña);

                    if (asesor != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }



        public string ObtenerRol(string matricula)
        {
            var usuario = _contexto.Usuario.FirstOrDefault(u => u.Matricula == matricula);

            if (usuario != null)
            {
                if (usuario.IdRol == 1)
                {
                    return "Administrador";
                }
                else if (usuario.IdRol == 2)
                {
                    return "Asesor";
                }
            }

            return null;
        }

    }
}