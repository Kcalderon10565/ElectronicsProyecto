
using System;
using System.IO;
using System.Linq;
using Electronics.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Electronics.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ElectronicsContext _context;
        private readonly IWebHostEnvironment _env;

        public UsuarioController(ElectronicsContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Se utilizan para almacenar las cookies e identificar al usuario con la sesión activa.
        // GET: /Usuario/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies.ContainsKey("NombreUsuario"))
                return RedirectToAction("Index", "Home");
            return View();
        }
        //Se inicia sesión con correo y contraseña, se mantiene la sesión activa por 2 horas
        // POST: /Usuario/Login
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
            //si los datos no coinciden con los de la base de datos de muestra el mensaje.
            ViewBag.Error = "Credenciales inválidas";
            return View();
        }

        //Con el siguiente método se cierra sesión del usuario conectado.
        // GET: /Usuario/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("IdUsuario");
            Response.Cookies.Delete("NombreUsuario");
            Response.Cookies.Delete("IdRol");
            return RedirectToAction("Index", "Home");
        }
        //Con el siguiente método accedemos al perfil, en donde nos extrae los datos
        //del usuarion con el que ingresamos para poder realizar una actualización.
        // GET: /Usuario/Perfil
        [HttpGet]
        public IActionResult Perfil()
        {
            if (!Request.Cookies.ContainsKey("IdUsuario"))
                return RedirectToAction("Login");

            int id = int.Parse(Request.Cookies["IdUsuario"]);

            var u = _context.Usuarios
                        .Include(x => x.FotoPerfil)
                        .FirstOrDefault(x => x.IdUsuario == id);
            if (u == null)
                return RedirectToAction("Login");

            var vm = new UsuarioPerfilViewModel
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                PrimerApellido = u.Primer_Apellido,
                SegundoApellido = u.Segundo_Apellido,
                Correo = u.Correo,
                RutaFotoActual = !string.IsNullOrEmpty(u.FotoPerfil?.Ruta)
                                  ? u.FotoPerfil.Ruta
                                  : "~/imagenes/perfil.png"
            };

            if (TempData["Mensaje"] != null)
                ViewBag.Mensaje = TempData["Mensaje"];

            return View(vm);
        }

        // POST: /Usuario/Perfil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Perfil(UsuarioPerfilViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var u = _context.Usuarios
                        .Include(x => x.FotoPerfil)
                        .FirstOrDefault(x => x.IdUsuario == vm.IdUsuario);
            if (u == null)
                return RedirectToAction("Login");

            // 1) Actualizar datos básicos
            u.Nombre = vm.Nombre;
            u.Primer_Apellido = vm.PrimerApellido;
            u.Segundo_Apellido = vm.SegundoApellido;
            u.Correo = vm.Correo;

            // 2) Cambiar contraseña
            if (!string.IsNullOrWhiteSpace(vm.NuevaContrasena))
                u.Contrasena = vm.NuevaContrasena;

            // 3) Subir nueva foto de perfil
            if (vm.FotoPerfil != null && vm.FotoPerfil.Length > 0)
            {
                var carpeta = Path.Combine(_env.WebRootPath, "fotosPerfil");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{u.IdUsuario}_{Path.GetFileName(vm.FotoPerfil.FileName)}";
                var rutaFisica = Path.Combine(carpeta, nombreArchivo);
                using var stream = new FileStream(rutaFisica, FileMode.Create);
                vm.FotoPerfil.CopyTo(stream);

                var img = new Imagenes
                {
                    Nombre = nombreArchivo,
                    Ruta = "/fotosPerfil/" + nombreArchivo
                };
                _context.Imagenes.Add(img);
                _context.SaveChanges();

                u.IdFotoPerfil = img.Id;
            }

            _context.SaveChanges();

            // 4) Refrescar cookie de NombreUsuario
            var opts = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            };
            Response.Cookies.Append("NombreUsuario", u.Nombre, opts);

            // 5) Muestra mensaje de exito
            TempData["Mensaje"] = "Perfil actualizado correctamente";
            return RedirectToAction("Perfil");
        }

        //Con el siguiente método un usuario puede registrarse.
        [HttpGet]
        public IActionResult Register()
        {
            // si ya está logueado, redirige a home
            if (Request.Cookies.ContainsKey("IdUsuario"))
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: /Usuario/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // comprobamos si el correo ya existe
            if (_context.Usuarios.Any(u => u.Correo == vm.Correo))
            {
                ModelState.AddModelError("Correo", "Ya existe un usuario con ese correo");
                return View(vm);
            }

            // creamos la entidad Usuario
            var usuario = new Usuario
            {
                Nombre = vm.Nombre,
                Primer_Apellido = vm.PrimerApellido,
                Segundo_Apellido = vm.SegundoApellido,
                Correo = vm.Correo,
                Contrasena = vm.Contrasena,
                IdRol = 2  // todos los nuevos son clientes
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            // auto-login: guardo cookies
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
    }
}

