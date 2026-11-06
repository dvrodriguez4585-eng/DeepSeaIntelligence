using DeepSeaIntelligence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Controllers
{
    public class FilaInventario
    {
        public int IdRecepcion { get; set; }
        public string Producto { get; set; } = "";
        public double Recibido { get; set; }
        public double Vendido { get; set; }
        public bool Rechazado { get; set; }
        public double Disponible => Rechazado ? 0 : Recibido - Vendido;
        public string Ubicacion { get; set; } = "Sin almacenar";
        public string Estado => Rechazado ? "Rechazado" : (Disponible > 0 ? "Disponible" : "Agotado");
    }

    public class InventariosController : Controller
    {
        private readonly AppDbContext _db;
        public InventariosController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index(string? buscar)
        {
            var productos = await _db.Productos.ToListAsync();
            var ventas = await _db.Ventas.ToListAsync();
            var almacenes = await _db.Almacenamientos.ToListAsync();
            var rech = await Reglas.IdsRechazados(_db);

            var filas = productos
                .Where(p => string.IsNullOrWhiteSpace(buscar) ||
                            p.ProductoRecibido.Contains(buscar, StringComparison.OrdinalIgnoreCase))
                .Select(p => new FilaInventario
                {
                    IdRecepcion = p.IdRecepcion,
                    Producto = p.ProductoRecibido,
                    Recibido = p.Peso,
                    Rechazado = rech.Contains(p.IdRecepcion),
                    Vendido = ventas.Where(v => v.IdRecepcion == p.IdRecepcion).Sum(v => v.CantidadVendida),
                    Ubicacion = almacenes.Where(a => a.IdRecepcion == p.IdRecepcion)
                                .OrderByDescending(a => a.FechaAlmacenamiento)
                                .Select(a => a.Ubicacion).FirstOrDefault() ?? "Sin almacenar"
                }).ToList();

            return View(filas);
        }
    }
}