using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Electronics.Models;
using System.Linq;

namespace Electronics.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ElectronicsContext _context;

        public ProductoController(ElectronicsContext context)
        {
            _context = context;
        }


        public IActionResult Catalogo()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Imagen)
                .ToList();

            return View(productos);
        }

        public IActionResult Smartphones()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Imagen)
                .Where(p => p.IdCategoria == 1)
                .ToList();

            return View(productos);
        }

        public IActionResult Laptops()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Imagen)
                .Where(p => p.IdCategoria == 2)
                .ToList();

            return View(productos);
        }

        public IActionResult Smartwatch()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Imagen)
                .Where(p => p.IdCategoria == 3)
                .ToList();

            return View(productos);
        }


        public IActionResult Audifonos()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Imagen)
                .Where(p => p.IdCategoria == 4)
                .ToList();

            return View(productos);
        }


        public IActionResult Tablets()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Imagen)
                .Where(p => p.IdCategoria == 5)
                .ToList();

            return View(productos);
        }

        public IActionResult Monitores()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Imagen)
                .Where(p => p.IdCategoria == 6)
                .ToList();

            return View(productos);
        }

        public IActionResult Accesorios()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Imagen)
                .Where(p => p.IdCategoria == 7)
                .ToList();

            return View(productos);
        }
    }
}
