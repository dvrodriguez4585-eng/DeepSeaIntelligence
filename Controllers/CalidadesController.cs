using DeepSeaIntelligence.Data;
using DeepSeaIntelligence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Controllers
{
    public class CalidadesController : Controller
    {
        private readonly AppDbContext _db;
        public CalidadesController(AppDbContext db) { _db = db; }

        private void CargarProductos(int? seleccionado = null)
        {
            ViewBag.Productos = new SelectList(_db.Productos.ToList(), "IdRecepcion", "ProductoRecibido", seleccionado);
        }

        public async Task<IActionResult> Index()
            => View(await _db.Calidades.Include(c => c.Recepcion).OrderByDescending(c => c.IdCalidad).ToListAsync());

        public IActionResult Create() { CargarProductos(); return View(new Calidad()); }

        [HttpPost]
        public async Task<IActionResult> Create(Calidad c)
        {
            ModelState.Remove("Recepcion");
            if (!ModelState.IsValid) { CargarProductos(c.IdRecepcion); return View(c); }
            _db.Calidades.Add(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Calidades.FindAsync(id);
            if (c == null) return NotFound();
            CargarProductos(c.IdRecepcion);
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Calidad c)
        {
            ModelState.Remove("Recepcion");
            if (!ModelState.IsValid) { CargarProductos(c.IdRecepcion); return View(c); }
            _db.Calidades.Update(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Calidades.FindAsync(id);
            if (c != null) { _db.Calidades.Remove(c); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}
