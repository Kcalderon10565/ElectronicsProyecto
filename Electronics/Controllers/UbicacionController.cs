using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Electronics.Models;

namespace Electronics.Controllers
{
    [ApiController]
    [Route("Ubicacion")]
    public class UbicacionController : Controller
    {
        //Basicamente acá se implementa la lógica para el geolocalizador, que nos muestra la sucursal más cercana.
        private readonly ElectronicsContext _context;
        public UbicacionController(ElectronicsContext context) => _context = context;

        [HttpGet("SucursalMasCercanaVista")]
        public IActionResult SucursalMasCercanaVista() =>
            View("SucursalMasCercanaVista");

        [HttpPost("SucursalMasCercanaVista")]
        public async Task<IActionResult> SucursalMasCercana([FromBody] Coordenadas cliente)
        {
            // ahora ToListAsync() estará disponible
            var sucursales = await _context.Sucursales.ToListAsync();

            var suc = sucursales
                .Select(s => new {
                    Info = s,
                    Dist = CalcularDistancia(
                             cliente.Latitud, cliente.Longitud,
                             s.Latitud, s.Longitud)
                })
                .OrderBy(x => x.Dist)
                .FirstOrDefault();

            if (suc == null) return NotFound();

            return Ok(new
            {
                nombre = suc.Info.Nombre,
                direccion = suc.Info.Direccion,
                distanciaKm = Math.Round(suc.Dist, 2)
            });
        }

        private static double CalcularDistancia(
            double lat1, double lon1,
            double lat2, double lon2)
        {
            const double R = 6371;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                  + Math.Cos(lat1 * Math.PI / 180)
                  * Math.Cos(lat2 * Math.PI / 180)
                  * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
