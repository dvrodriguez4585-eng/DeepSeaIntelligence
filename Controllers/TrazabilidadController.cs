using System.Text;
using DeepSeaIntelligence.Data;
using DeepSeaIntelligence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace DeepSeaIntelligence.Controllers
{
    [AllowAnonymous]
    public class TrazabilidadController : Controller
    {
        private readonly AppDbContext _db;
        public TrazabilidadController(AppDbContext db) { _db = db; }

        private async Task<(Calidad? cal, Conservacion? con, Almacenamiento? alm, List<string> motivos)> Evaluar(ProductoMarino p)
        {
            var cal = await _db.Calidades.Where(c => c.IdRecepcion == p.IdRecepcion)
                        .OrderByDescending(c => c.FechaEvaluacion).ThenByDescending(c => c.IdCalidad).FirstOrDefaultAsync();
            var con = await _db.Conservaciones.Where(c => c.IdRecepcion == p.IdRecepcion)
                        .OrderByDescending(c => c.FechaConservacion).FirstOrDefaultAsync();
            var alm = await _db.Almacenamientos.Where(a => a.IdRecepcion == p.IdRecepcion)
                        .OrderByDescending(a => a.FechaAlmacenamiento).FirstOrDefaultAsync();

            var motivos = new List<string>();
            if (cal == null) motivos.Add("No tiene evaluacion de calidad registrada.");
            else if (cal.Resultado != "Aprobado") motivos.Add("La calidad fue rechazada.");

            if (con == null) motivos.Add("No tiene registro de conservacion.");
            else
            {
                double max = con.MetodoConservacion == "Congelado" ? -15 : 4;
                if (con.Temperatura > max)
                    motivos.Add($"La temperatura registrada ({con.Temperatura} C) supera el maximo permitido ({max} C).");
            }
            if (alm == null) motivos.Add("No tiene registro de almacenamiento.");
            return (cal, con, alm, motivos);
        }

        // Pagina web de trazabilidad (si hay acceso a la red)
        public async Task<IActionResult> Ver(string id)
        {
            var p = await _db.Productos.FirstOrDefaultAsync(x => x.CodigoQR == id);
            if (p == null) return NotFound();
            var (cal, con, alm, motivos) = await Evaluar(p);

            ViewBag.Calidad = cal;
            ViewBag.Conservacion = con;
            ViewBag.Almacenamiento = alm;
            ViewBag.Adecuado = motivos.Count == 0;
            ViewBag.Motivos = motivos;
            return View(p);
        }

                public async Task<IActionResult> Qr(string id, [FromServices] IConfiguration config)
        {
            var p = await _db.Productos.FirstOrDefaultAsync(x => x.CodigoQR == id);
            if (p == null) return NotFound();

            var baseUrl = config["SitioUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl)) baseUrl = $"{Request.Scheme}://{Request.Host}";
            var url = $"{baseUrl.TrimEnd('/')}/Trazabilidad/Ver/{id}";

            using var gen = new QRCodeGenerator();
            var datos = gen.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var png = new PngByteQRCode(datos).GetGraphic(10);
            return File(png, "image/png");
        }
        }
        }
        