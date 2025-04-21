using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Electronics.Models;

namespace Electronics.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ElectronicsContext _context;

        public CarritoController(ElectronicsContext context)
        {
            _context = context;
        }

        // GET: /Carrito
        public IActionResult Index()
        {
            if (!Request.Cookies.ContainsKey("IdUsuario"))
                return RedirectToAction("Login", "Usuario");
        //Se utilizan los cookies que mantienen la sesión iniciada para asociar un usuario en específico
        //a los productos del carrito y se muestran.
            int idUsuario = int.Parse(Request.Cookies["IdUsuario"]);
            var items = _context.Carritos
                                .Include(c => c.Producto)
                                .Where(c => c.IdUsuario == idUsuario)
                                .ToList();

            ViewBag.Total = items.Sum(i => i.Cantidad * i.Producto.Precio);
            ViewBag.NumeroProductos = items.Sum(i => i.Cantidad);
            return View(items);
        }
        //En este método se añaden los productos a un usuario en específico utilizando las cookies.

        // POST: /Carrito/Add
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(int productId)
        {
            if (!Request.Cookies.ContainsKey("IdUsuario"))
                return RedirectToAction("Login", "Usuario");

            int idUsuario = int.Parse(Request.Cookies["IdUsuario"]);
            var item = _context.Carritos
                              .FirstOrDefault(c => c.IdUsuario == idUsuario && c.IdProducto == productId);

            if (item == null)
            {
                _context.Carritos.Add(new Carrito
                {
                    IdUsuario = idUsuario,
                    IdProducto = productId,
                    Cantidad = 1
                });
            }
            else
            {
                item.Cantidad++;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //Este método se utiliza para eliminar productos del carrito.

        // POST: /Carrito/Remove
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            if (!Request.Cookies.ContainsKey("IdUsuario"))
                return RedirectToAction("Login", "Usuario");

            int idUsuario = int.Parse(Request.Cookies["IdUsuario"]);
            var item = _context.Carritos
                               .FirstOrDefault(c => c.IdCarrito == id && c.IdUsuario == idUsuario);

            if (item != null)
            {
                _context.Carritos.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Carrito/Checkout
        [HttpGet]
        public IActionResult Checkout()
        {
            if (!Request.Cookies.ContainsKey("IdUsuario"))
                return RedirectToAction("Login", "Usuario");

            int idUsuario = int.Parse(Request.Cookies["IdUsuario"]);
            var items = _context.Carritos
                                .Include(c => c.Producto)
                                .Where(c => c.IdUsuario == idUsuario)
                                .ToList();

            var vm = new CheckoutViewModel
            {
                Items = items,
                Total = items.Sum(i => i.Cantidad * i.Producto.Precio)
            };
            return View(vm);
        }

        // POST: /Carrito/Checkout
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutViewModel vm)
        {
            if (!Request.Cookies.ContainsKey("IdUsuario"))
                return RedirectToAction("Login", "Usuario");

            int idUsuario = int.Parse(Request.Cookies["IdUsuario"]);
            var items = _context.Carritos
                                .Include(c => c.Producto)
                                .Where(c => c.IdUsuario == idUsuario)
                                .ToList();

            // volver a poblar antes de validar
            vm.Items = items;
            vm.Total = items.Sum(i => i.Cantidad * i.Producto.Precio);

            if (!ModelState.IsValid)
                return View(vm);

            // crear el pedido (fecha como string)
            var pedido = new Pedido
            {
                IdUsuario = idUsuario,
                Fecha = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                Total = vm.Total,
                NumeroTarjeta = vm.NumeroTarjeta,
                FechaVencimiento = vm.FechaVencimiento,
                CCV = vm.CCV
            };
            _context.Pedidos.Add(pedido);

            // vaciar carrito
            _context.Carritos.RemoveRange(items);
            _context.SaveChanges();

            return RedirectToAction(nameof(Confirmation), new { id = pedido.IdPedido });
        }

        // GET: /Carrito/Confirmation/5
        public IActionResult Confirmation(int id)
        {
            ViewBag.IdPedido = id;
            return View();
        }
    }
}
