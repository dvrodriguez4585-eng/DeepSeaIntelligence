using DeepSeaIntelligence.Data;
using DeepSeaIntelligence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _db;
        public ClientesController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
            => View(await _db.Clientes.OrderByDescending(c => c.IdCliente).ToListAsync());

        public IActionResult Create() => View(new Cliente());

        [HttpPost]
        public async Task<IActionResult> Create(Cliente c)
        {
            if (!ModelState.IsValid) return View(c);
            _db.Clientes.Add(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Clientes.FindAsync(id);
            return c == null ? NotFound() : View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cliente c)
        {
            if (!ModelState.IsValid) return View(c);
            _db.Clientes.Update(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Clientes.FindAsync(id);
            if (c != null)
            {
                bool tieneVentas = await _db.Ventas.AnyAsync(v => v.IdCliente == id);
                if (!tieneVentas) { _db.Clientes.Remove(c); await _db.SaveChangesAsync(); }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}