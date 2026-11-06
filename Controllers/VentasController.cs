using DeepSeaIntelligence.Data;
using DeepSeaIntelligence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Controllers
{
    public class VentasController : Controller
    {
        private const double PedidoMinimo = 1;
        private const double LimiteDistribucion = 100;

        private readonly AppDbContext _db;
        public VentasController(AppDbContext db) { _db = db; }

        private async Task Cargar(int? producto = null, int? cliente = null)
        {
            var rech = await Reglas.IdsRechazados(_db);
            var prods = (await _db.Productos.ToListAsync()).Where(p => !rech.Contains(p.IdRecepcion));
            ViewBag.Productos = new SelectList(prods, "IdRecepcion", "ProductoRecibido", producto);
            ViewBag.Clientes = new SelectList(await _db.Clientes.ToListAsync(), "IdCliente", "NombreCliente", cliente);
            ViewBag.PedidoMinimo = PedidoMinimo;
            ViewBag.LimiteDistribucion = LimiteDistribucion;
        }

        private async Task<double> Disponible(int idRecepcion)
        {
            var p = await _db.Productos.FindAsync(idRecepcion);
            if (p == null) return 0;
            var vendido = await _db.Ventas.Where(v => v.IdRecepcion == idRecepcion)
                .SumAsync(v => (double?)v.CantidadVendida) ?? 0;
            return p.Peso - vendido;
        }

        public async Task<IActionResult> Index()
        {
            var ventas = await _db.Ventas.OrderByDescending(v => v.IdVenta).ToListAsync();
            ViewBag.Productos = await _db.Productos.ToDictionaryAsync(p => p.IdRecepcion, p => p.ProductoRecibido);
            ViewBag.Clientes = await _db.Clientes.ToDictionaryAsync(c => c.IdCliente, c => c.NombreCliente);
            var coms = await _db.Comercializaciones.ToListAsync();
            ViewBag.Coms = coms.GroupBy(c => c.IdVenta).ToDictionary(g => g.Key, g => g.Last());
            return View(ventas);
        }

        public async Task<IActionResult> Create()
        {
            await Cargar();
            return View(new Venta());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Venta v, string canal = "Local", string destino = "", string responsable = "")
        {
            ModelState.Remove("Recepcion");
            ModelState.Remove("Cliente");

            if ((await Reglas.IdsRechazados(_db)).Contains(v.IdRecepcion))
                ModelState.AddModelError("", "Este producto fue rechazado por calidad y no se puede vender.");

            double disp = await Disponible(v.IdRecepcion);
            if (v.CantidadVendida < PedidoMinimo)
                ModelState.AddModelError("", $"El pedido minimo es de {PedidoMinimo} kg.");
            else if (v.CantidadVendida > disp)
                ModelState.AddModelError("", $"Stock insuficiente. Disponible: {disp} kg.");

            if ((canal == "Distribuidores" || canal == "Exportación") && v.CantidadVendida > LimiteDistribucion)
                ModelState.AddModelError("", $"Para {canal} el maximo por venta es de {LimiteDistribucion} kg.");

            if (!ModelState.IsValid)
            {
                await Cargar(v.IdRecepcion, v.IdCliente);
                ViewBag.Canal = canal; ViewBag.Destino = destino; ViewBag.Responsable = responsable;
                return View(v);
            }

            v.PrecioTotal = v.CantidadVendida * v.PrecioUnitario;
            _db.Ventas.Add(v);
            await _db.SaveChangesAsync();

            _db.Comercializaciones.Add(new Comercializacion
            {
                IdVenta = v.IdVenta,
                CanalVenta = canal,
                Destino = destino ?? "",
                FechaComercializacion = v.FechaVenta,
                Responsable = responsable ?? ""
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var v = await _db.Ventas.FindAsync(id);
            if (v != null)
            {
                var coms = await _db.Comercializaciones.Where(c => c.IdVenta == id).ToListAsync();
                _db.Comercializaciones.RemoveRange(coms);
                _db.Ventas.Remove(v);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}