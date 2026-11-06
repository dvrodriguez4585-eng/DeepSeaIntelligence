using DeepSeaIntelligence.Data;
using DeepSeaIntelligence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

            var productos = await _db.Productos.ToListAsync();
            var ventas = await _db.Ventas.ToListAsync();
            var conservaciones = await _db.Conservaciones.Include(c => c.Recepcion).ToListAsync();

            // Alertas: última conservación de cada producto con temperatura fuera de rango
            var alertas = conservaciones
                .GroupBy(c => c.IdRecepcion)
                .Select(g => g.OrderByDescending(x => x.FechaConservacion).First())
                .Where(c => c.Temperatura > (c.MetodoConservacion == "Congelado" ? -15 : 4))
                .Select(c => $"{c.Recepcion?.ProductoRecibido} - temperatura fuera de rango ({c.Temperatura} °C)")
                .ToList();

            ViewBag.TotalProductos = productos.Count;
            ViewBag.Stock = productos.Sum(p => p.Peso) - ventas.Sum(v => v.CantidadVendida);
            ViewBag.VentasMes = ventas.Where(v => v.FechaVenta >= inicioMes).Sum(v => v.PrecioTotal);
            ViewBag.Alertas = alertas;

            var ultimas = productos.OrderByDescending(p => p.IdRecepcion).Take(5).ToList();
            return View(ultimas);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}