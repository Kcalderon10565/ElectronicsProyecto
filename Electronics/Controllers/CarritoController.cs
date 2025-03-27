using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Electronics.Controllers
{
    public class CarritoController : Controller
    {
        // GET: Carrito
        public ActionResult Agregar()
        {
            return View();
        }

        // GET: Carrito/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Carrito/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrito/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Carrito/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Carrito/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Carrito/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Carrito/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
