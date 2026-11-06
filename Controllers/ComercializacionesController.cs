using DeepSeaIntelligence.Data;
using DeepSeaIntelligence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Controllers
{
    public class ComercializacionesController : Controller
    {
        private readonly AppDbContext _db;
        public ComercializacionesController(AppDbContext db) { _db = db; }

        private void CargarVentas(int? seleccionada = null)
        {
            var ventas = _db.Ventas.Include(v => v.Recepcion).ToList()
                .Select(v => new { v.IdVenta, Texto = $"Venta #{v.IdVenta} - {v.Recepcion?.ProductoRecibido} ({v.CantidadVendida} kg)" });
            ViewBag.Ventas = new SelectList(ventas, "IdVenta", "Texto", seleccionada);
        }

        public async Task<IActionResult> Index()
            => View(await _db.Comercializaciones.Include(c => c.Venta).ThenInclude(v => v!.Recepcion)
                    .OrderByDescending(c => c.IdComercializacion).ToListAsync());

        public IActionResult Create() { CargarVentas(); return View(new Comercializacion()); }

        [HttpPost]
        public async Task<IActionResult> Create(Comercializacion c)
        {
            ModelState.Remove("Venta");
            if (!ModelState.IsValid) { CargarVentas(c.IdVenta); return View(c); }
            _db.Comercializaciones.Add(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Comercializaciones.FindAsync(id);
            if (c == null) return NotFound();
            CargarVentas(c.IdVenta);
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Comercializacion c)
        {
            ModelState.Remove("Venta");
            if (!ModelState.IsValid) { CargarVentas(c.IdVenta); return View(c); }
            _db.Comercializaciones.Update(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Comercializaciones.FindAsync(id);
            if (c != null) { _db.Comercializaciones.Remove(c); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}