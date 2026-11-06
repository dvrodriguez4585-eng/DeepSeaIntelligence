using DeepSeaIntelligence.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeepSeaIntelligence.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProductoMarino> Productos { get; set; }
        public DbSet<Calidad> Calidades { get; set; }
        public DbSet<Conservacion> Conservaciones { get; set; }
        public DbSet<Almacenamiento> Almacenamientos { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Comercializacion> Comercializaciones { get; set; }
    }
}