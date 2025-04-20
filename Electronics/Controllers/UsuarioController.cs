using System;
using System.Linq;
using Electronics.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronics.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ElectronicsContext _context;
        public UsuarioController(ElectronicsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si ya hay sesión iniciada, redirigimos
            if (Request.Cookies.ContainsKey("NombreUsuario"))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Email, string Password)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Correo == Email && u.Contrasena == Password);

            if (usuario != null)
            {
                var opciones = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(2)
                };

                Response.Cookies.Append("IdUsuario", usuario.IdUsuario.ToString(), opciones);
                Response.Cookies.Append("NombreUsuario", usuario.Nombre, opciones);
                Response.Cookies.Append("IdRol", usuario.IdRol.ToString(), opciones);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales inválidas";
            return View();
        }

        // GET: /Usuario/Logout
        public IActionResult Logout()
        {
            Response.Cookies.Delete("IdUsuario");
            Response.Cookies.Delete("NombreUsuario");
            Response.Cookies.Delete("IdRol");
            return RedirectToAction("Index", "Home");
        }

        // GET: /Usuario/Perfil
        public IActionResult Perfil()
        {
            if (!Request.Cookies.ContainsKey("IdUsuario"))
                return RedirectToAction("Login");

            var id = int.Parse(Request.Cookies["IdUsuario"]);
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null)
                return RedirectToAction("Login");

            return View(usuario);
        }
    }
}
