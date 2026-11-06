using DeepSeaIntelligence.Data;
using DeepSeaIntelligence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _db;
        public ProductosController(AppDbContext db) { _db = db; }

        private static void AplicarFecha(ProductoMarino p)
        {
            if (p.TipoFecha == "Ingreso") { p.FechaIngreso = p.Fecha; p.FechaCaptura = null; }
            else { p.FechaCaptura = p.Fecha; p.FechaIngreso = null; }
        }

        public async Task<IActionResult> Index(string? buscar)
        {
            var q = _db.Productos.AsQueryable();
            if (!string.IsNullOrWhiteSpace(buscar))
                q = q.Where(p => p.ProductoRecibido.Contains(buscar));
            ViewBag.Rechazados = await Reglas.IdsRechazados(_db);
            return View(await q.OrderByDescending(p => p.IdRecepcion).ToListAsync());
        }

        public IActionResult Create() => View(new ProductoMarino());

        [HttpPost]
        public async Task<IActionResult> Create(ProductoMarino p)
        {
            if (!ModelState.IsValid) return View(p);
            AplicarFecha(p);
            p.CodigoQR = Guid.NewGuid().ToString("N");
            _db.Productos.Add(p);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var p = await _db.Productos.FindAsync(id);
            if (p == null) return NotFound();
            p.TipoFecha = p.FechaCaptura != null ? "Captura" : "Ingreso";
            p.Fecha = p.FechaCaptura ?? p.FechaIngreso ?? DateTime.Today;
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductoMarino p)
        {
            if (!ModelState.IsValid) return View(p);
            AplicarFecha(p);
            _db.Productos.Update(p);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Productos.FindAsync(id);
            if (p != null) { _db.Productos.Remove(p); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}