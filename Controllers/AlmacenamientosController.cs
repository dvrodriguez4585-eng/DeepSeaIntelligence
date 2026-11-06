using DeepSeaIntelligence.Data;
using DeepSeaIntelligence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Controllers
{
    public class AlmacenamientosController : Controller
    {
        private readonly AppDbContext _db;
        public AlmacenamientosController(AppDbContext db) { _db = db; }

        private async Task CargarProductos(int? seleccionado = null)
        {
            var rech = await Reglas.IdsRechazados(_db);
            var lista = (await _db.Productos.ToListAsync()).Where(p => !rech.Contains(p.IdRecepcion));
            ViewBag.Productos = new SelectList(lista, "IdRecepcion", "ProductoRecibido", seleccionado);
        }

        private async Task<bool> Validar(Almacenamiento a)
        {
            ModelState.Remove("Recepcion");
            if ((await Reglas.IdsRechazados(_db)).Contains(a.IdRecepcion))
                ModelState.AddModelError("", "Este producto fue rechazado por calidad y no se puede almacenar.");
            return ModelState.IsValid;
        }

        public async Task<IActionResult> Index()
            => View(await _db.Almacenamientos.Include(a => a.Recepcion).OrderByDescending(a => a.IdAlmacenamiento).ToListAsync());

        public async Task<IActionResult> Create() { await CargarProductos(); return View(new Almacenamiento()); }

        [HttpPost]
        public async Task<IActionResult> Create(Almacenamiento a)
        {
            if (!await Validar(a)) { await CargarProductos(a.IdRecepcion); return View(a); }
            _db.Almacenamientos.Add(a);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var a = await _db.Almacenamientos.FindAsync(id);
            if (a == null) return NotFound();
            await CargarProductos(a.IdRecepcion);
            return View(a);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Almacenamiento a)
        {
            if (!await Validar(a)) { await CargarProductos(a.IdRecepcion); return View(a); }
            _db.Almacenamientos.Update(a);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var a = await _db.Almacenamientos.FindAsync(id);
            if (a != null) { _db.Almacenamientos.Remove(a); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}