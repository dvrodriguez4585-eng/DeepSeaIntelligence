using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeepSeaIntelligence.Models
{
    public class ProductoMarino
    {
        [Key] public int IdRecepcion { get; set; }
        [Required, Display(Name = "Producto recibido")] public string ProductoRecibido { get; set; } = "";
        [Display(Name = "Peso (kg)")] public double Peso { get; set; }
        [Display(Name = "Fecha de ingreso"), DataType(DataType.Date)] public DateTime? FechaIngreso { get; set; }
        [Display(Name = "Fecha de captura"), DataType(DataType.Date)] public DateTime? FechaCaptura { get; set; }
        [Display(Name = "Lugar de origen")] public string LugarOrigen { get; set; } = "";
        public string? Observaciones { get; set; }
        [Display(Name = "Tipo de proveedor")] public string TipoProveedor { get; set; } = "Pescador";
        [Display(Name = "Nombre del proveedor")] public string NombreProveedor { get; set; } = "";
        [Display(Name = "Ubicación del proveedor")] public string? UbicacionProveedor { get; set; }
        [Display(Name = "Teléfono")] public string? TelefonoProveedor { get; set; }
        [Display(Name = "Correo")] public string? EmailProveedor { get; set; }
        public string CodigoQR { get; set; } = Guid.NewGuid().ToString("N");

        // Solo para el formulario (no se guardan en la base de datos)
        [NotMapped] public string TipoFecha { get; set; } = "Captura";
        [NotMapped, DataType(DataType.Date)] public DateTime Fecha { get; set; } = DateTime.Today;
    }

    public class Calidad
    {
        [Key] public int IdCalidad { get; set; }
        [Display(Name = "Recepción")] public int IdRecepcion { get; set; }
        public ProductoMarino? Recepcion { get; set; }
        public string Color { get; set; } = "";
        public string Olor { get; set; } = "";
        public string Textura { get; set; } = "";
        public string Resultado { get; set; } = "Aprobado";
        [Display(Name = "Fecha de evaluación"), DataType(DataType.Date)] public DateTime FechaEvaluacion { get; set; } = DateTime.Today;
    }

    public class Conservacion
    {
        [Key] public int IdConservacion { get; set; }
        [Display(Name = "Recepción")] public int IdRecepcion { get; set; }
        public ProductoMarino? Recepcion { get; set; }
        [Display(Name = "Método")] public string MetodoConservacion { get; set; } = "Hielo";
        [Display(Name = "Fecha"), DataType(DataType.Date)] public DateTime FechaConservacion { get; set; } = DateTime.Today;
        [Display(Name = "Duración estimada (h)")] public int DuracionEstimada { get; set; }
        [Display(Name = "Temperatura (°C)")] public double Temperatura { get; set; }
        public string Responsable { get; set; } = "";
    }

    public class Almacenamiento
    {
        [Key] public int IdAlmacenamiento { get; set; }
        [Display(Name = "Recepción")] public int IdRecepcion { get; set; }
        public ProductoMarino? Recepcion { get; set; }
        [Display(Name = "Tipo")] public string TipoAlmacenamiento { get; set; } = "Frío";
        [Display(Name = "Ubicación")] public string Ubicacion { get; set; } = "";
        [Display(Name = "Fecha"), DataType(DataType.Date)] public DateTime FechaAlmacenamiento { get; set; } = DateTime.Today;
        public string? Condiciones { get; set; }
        public string Responsable { get; set; } = "";
    }

    public class Inventario
    {
        [Key] public int IdInventario { get; set; }
        [Display(Name = "Almacenamiento")] public int IdAlmacenamiento { get; set; }
        public Almacenamiento? Almacenamiento { get; set; }
        [Display(Name = "Cantidad disponible")] public double CantidadDisponible { get; set; }
        [Display(Name = "Unidad")] public string UnidadMedida { get; set; } = "kg";
        [Display(Name = "Actualización"), DataType(DataType.Date)] public DateTime FechaActualizacion { get; set; } = DateTime.Today;
        [Display(Name = "Estado")] public string EstadoStock { get; set; } = "Disponible";
        public string? Ubicacion { get; set; }
        public string? Observaciones { get; set; }
    }

    public class Cliente
    {
        [Key] public int IdCliente { get; set; }
        [Required, Display(Name = "Nombre")] public string NombreCliente { get; set; } = "";
        [Display(Name = "Tipo de documento")] public string TipoIdentificacion { get; set; } = "CC";
        [Display(Name = "Número de documento")] public string NumeroIdentificacion { get; set; } = "";
        [Display(Name = "Dirección")] public string? Direccion { get; set; }
        [Display(Name = "Teléfono")] public string? Telefono { get; set; }
        public string? Email { get; set; }
    }

    public class Venta
    {
        [Key] public int IdVenta { get; set; }
        [Display(Name = "Recepción")] public int IdRecepcion { get; set; }
        public ProductoMarino? Recepcion { get; set; }
        [Display(Name = "Cliente")] public int IdCliente { get; set; }
        public Cliente? Cliente { get; set; }
        [Display(Name = "Fecha"), DataType(DataType.Date)] public DateTime FechaVenta { get; set; } = DateTime.Today;
        [Display(Name = "Cantidad")] public double CantidadVendida { get; set; }
        [Display(Name = "Precio unitario")] public double PrecioUnitario { get; set; }
        [Display(Name = "Total")] public double PrecioTotal { get; set; }
        [Display(Name = "Método de pago")] public string MetodoPago { get; set; } = "Efectivo";
        public string? Observaciones { get; set; }

        [NotMapped] public string Codigo => $"V-{IdVenta:D5}";
    }

    public class Comercializacion
    {
        [Key] public int IdComercializacion { get; set; }
        [Display(Name = "Venta")] public int IdVenta { get; set; }
        public Venta? Venta { get; set; }
        [Display(Name = "Canal")] public string CanalVenta { get; set; } = "Local";
        public string Destino { get; set; } = "";
        [Display(Name = "Fecha"), DataType(DataType.Date)] public DateTime FechaComercializacion { get; set; } = DateTime.Today;
        public string Responsable { get; set; } = "";
        public string? Observaciones { get; set; }
    }
}