using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Data
{
    public static class Reglas
    {
        public static async Task<HashSet<int>> IdsRechazados(AppDbContext db)
        {
            var cal = await db.Calidades.ToListAsync();
            return cal.GroupBy(c => c.IdRecepcion)
                .Select(g => g.OrderByDescending(x => x.FechaEvaluacion).ThenByDescending(x => x.IdCalidad).First())
                .Where(c => c.Resultado == "Rechazado")
                .Select(c => c.IdRecepcion)
                .ToHashSet();
        }
    }
}