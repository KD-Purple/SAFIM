using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SAFIM.Models;
using System.Data;

namespace SAFIM.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministradorController : Controller
    {
        private readonly SistemaAsesoriasContext _contexto;
        public AdministradorController(SistemaAsesoriasContext contexto)
        {
            _contexto = contexto;
        }


        public IActionResult Index()
        {
            return View();
        }


    }
}