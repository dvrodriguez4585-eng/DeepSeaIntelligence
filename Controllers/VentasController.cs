using DeepSeaIntelligence.Data;
using DeepSeaIntelligence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Controllers
{
    public class VentasController : Controller
    {
        private readonly AppDbContext _db;
        public VentasController(AppDbContext db) { _db = db; }

        private void Cargar(int? producto = null, int? cliente = null)
        {
            ViewBag.Productos = new SelectList(_db.Productos.ToList(), "IdRecepcion", "ProductoRecibido", producto);
            ViewBag.Clientes = new SelectList(_db.Clientes.ToList(), "IdCliente", "NombreCliente", cliente);
        }

        private async Task<double> Disponible(int idRecepcion, int? ignorarVenta = null)
        {
            var p = await _db.Productos.FindAsync(idRecepcion);
            if (p == null) return 0;
            var vendido = await _db.Ventas
                .Where(v => v.IdRecepcion == idRecepcion && v.IdVenta != ignorarVenta)
                .SumAsync(v => (double?)v.CantidadVendida) ?? 0;
            return p.Peso - vendido;
        }

        public async Task<IActionResult> Index()
            => View(await _db.Ventas.Include(v => v.Recepcion).Include(v => v.Cliente)
                    .OrderByDescending(v => v.IdVenta).ToListAsync());

        public IActionResult Create() { Cargar(); return View(new Venta()); }

        [HttpPost]
        public async Task<IActionResult> Create(Venta v)
        {
            ModelState.Remove("Recepcion"); ModelState.Remove("Cliente");
            double disp = await Disponible(v.IdRecepcion);
            if (v.CantidadVendida <= 0) ModelState.AddModelError("", "La cantidad debe ser mayor a 0.");
            else if (v.CantidadVendida > disp) ModelState.AddModelError("", $"Stock insuficiente. Disponible: {disp} kg.");
            if (!ModelState.IsValid) { Cargar(v.IdRecepcion, v.IdCliente); return View(v); }

            v.PrecioTotal = v.CantidadVendida * v.PrecioUnitario;
            _db.Ventas.Add(v);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var v = await _db.Ventas.FindAsync(id);
            if (v != null)
            {
                bool tieneCom = await _db.Comercializaciones.AnyAsync(c => c.IdVenta == id);
                if (!tieneCom) { _db.Ventas.Remove(v); await _db.SaveChangesAsync(); }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}