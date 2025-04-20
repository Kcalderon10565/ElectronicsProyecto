using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Electronics.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace Electronics.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ElectronicsContext _context;
        public ProductoController(ElectronicsContext context)
            => _context = context;

        // método auxiliar para la query base
        private IQueryable<Productos> QueryBase()
            => _context.Productos
                       .Include(p => p.Categoria)
                       .Include(p => p.Imagen);

        // método auxiliar que filtra por nombre de categoría
        // y devuelve la vista cuyo nombre coincide con viewName
        private IActionResult FiltrarYMostrar(string nombreCategoria, string viewName)
        {
            var lista = QueryBase()
                .Where(p => p.Categoria.Nombre == nombreCategoria)
                .ToList();

            // reconstruye tu lógica de EsAdmin
            var idRol = HttpContext.Request.Cookies.ContainsKey("IdRol")
                ? int.Parse(HttpContext.Request.Cookies["IdRol"])
                : (int?)null;
            ViewBag.EsAdmin = idRol == 1;

            ViewBag.CategoriaSeleccionada = nombreCategoria;
            return View(viewName, lista);
        }

        // ahora, para cada categoría basta con un método
        public IActionResult Smartphones() => FiltrarYMostrar("Smartphones", "Smartphones");
        public IActionResult Laptops() => FiltrarYMostrar("Laptops", "Laptops");
        public IActionResult Smartwatch() => FiltrarYMostrar("Smartwatch", "Smartwatch");
        public IActionResult Audifonos() => FiltrarYMostrar("Audifonos", "Audifonos");
        public IActionResult Tablets() => FiltrarYMostrar("Tablets", "Tablets");
        public IActionResult Monitores() => FiltrarYMostrar("Monitores", "Monitores");
        public IActionResult Accesorios() => FiltrarYMostrar("Accesorios", "Accesorios");


        public IActionResult Catalogo()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria) // Relación con Categoria
                .Include(p => p.Imagen)    // Relación con Imagen
                .ToList();

            var idRol = HttpContext.Request.Cookies.ContainsKey("IdRol")
                ? int.Parse(HttpContext.Request.Cookies["IdRol"])
                : (int?)null;

            ViewBag.EsAdmin = (idRol == 1); // true si es admin

            return View(productos);
        }

        // Acción para crear un producto
        public IActionResult Crear()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias.ToList(), "IdCategoria", "Nombre");
            ViewBag.Accion = "Crear";
            ViewBag.Edicion = false;
            return View("Formulario", new Productos()); // Usa la vista "Formulario"
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Productos producto, IFormFile imagenArchivo)
        {
            if (ModelState.IsValid)
            {
                // Si se sube una nueva imagen
                if (imagenArchivo != null && imagenArchivo.Length > 0)
                {
                    var rutaImagen = Path.Combine("wwwroot/imagenes", imagenArchivo.FileName);
                    using (var stream = new FileStream(rutaImagen, FileMode.Create))
                    {
                        imagenArchivo.CopyTo(stream);
                    }

                    var imagen = new Imagenes
                    {
                        Ruta = "/imagenes/" + imagenArchivo.FileName
                    };

                    _context.Imagenes.Add(imagen);
                    _context.SaveChanges();

                    // Asocia la imagen al producto
                    producto.Imagen = imagen;
                }

                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction("Catalogo");
            }

            ViewBag.Categorias = new SelectList(_context.Categorias.ToList(), "IdCategoria", "Nombre");
            ViewBag.Accion = "Crear";
            ViewBag.Edicion = false;
            return View("Formulario", producto);
        }

        // Acción para editar un producto
        public IActionResult Editar(int id)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.IdProducto == id);
            if (producto == null) return NotFound();

            ViewBag.Categorias = new SelectList(_context.Categorias.ToList(), "IdCategoria", "Nombre", producto.IdCategoria);
            ViewBag.Accion = "Editar";
            ViewBag.Edicion = true;
            return View("Formulario", producto); // Usa la vista "Formulario"
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Productos producto, IFormFile imagenArchivo)
        {
            if (ModelState.IsValid)
            {
                var productoExistente = _context.Productos
                    .Include(p => p.Imagen)
                    .FirstOrDefault(p => p.IdProducto == producto.IdProducto);

                if (productoExistente == null)
                    return NotFound();

                // Actualizar datos básicos
                productoExistente.Nombre = producto.Nombre;
                productoExistente.Descripcion = producto.Descripcion;
                productoExistente.Precio = producto.Precio;
                productoExistente.Stock = producto.Stock;
                productoExistente.IdCategoria = producto.IdCategoria;

                // Si hay nueva imagen, reemplazar
                if (imagenArchivo != null && imagenArchivo.Length > 0)
                {
                    // Borrar la imagen anterior (si existe)
                    if (productoExistente.Imagen != null)
                    {
                        var rutaAnterior = Path.Combine("wwwroot", productoExistente.Imagen.Ruta.TrimStart('/'));
                        if (System.IO.File.Exists(rutaAnterior))
                        {
                            System.IO.File.Delete(rutaAnterior);
                        }

                        // Eliminar de la base de datos
                        _context.Imagenes.Remove(productoExistente.Imagen);
                    }

                    // Guardar la nueva imagen
                    var rutaNueva = Path.Combine("wwwroot/imagenes", imagenArchivo.FileName);
                    using (var stream = new FileStream(rutaNueva, FileMode.Create))
                    {
                        imagenArchivo.CopyTo(stream);
                    }

                    var nuevaImagen = new Imagenes
                    {
                        Ruta = "/imagenes/" + imagenArchivo.FileName
                    };

                    _context.Imagenes.Add(nuevaImagen);
                    productoExistente.Imagen = nuevaImagen;
                }

                _context.SaveChanges();
                return RedirectToAction("Catalogo");
            }

            ViewBag.Categorias = new SelectList(_context.Categorias.ToList(), "IdCategoria", "Nombre", producto.IdCategoria);
            ViewBag.Accion = "Editar";
            ViewBag.Edicion = true;
            return View("Formulario", producto);
        }

        // GET: /Producto/Eliminar/5
public IActionResult Eliminar(int id)
        {
            // 1) Busca el producto (incluyendo la imagen si quieres borrarla también)
            var producto = _context.Productos
                            .Include(p => p.Imagen)
                            .FirstOrDefault(p => p.IdProducto == id);
            if (producto == null) return NotFound();

            // 2) (Opcional) Borra el archivo físico de la imagen
            if (producto.Imagen != null)
            {
                var rutaFisica = Path.Combine("wwwroot", producto.Imagen.Ruta.TrimStart('/'));
                if (System.IO.File.Exists(rutaFisica))
                    System.IO.File.Delete(rutaFisica);

                // y elimina el registro de imagen
                _context.Imagenes.Remove(producto.Imagen);
            }

            // 3) Borra el producto
            _context.Productos.Remove(producto);
            _context.SaveChanges();

            // 4) Vuelve al catálogo
            return RedirectToAction("Catalogo");
        }

    } }
