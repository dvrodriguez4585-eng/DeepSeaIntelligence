using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeepSeaIntelligence.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreCliente = table.Column<string>(type: "text", nullable: false),
                    TipoIdentificacion = table.Column<string>(type: "text", nullable: false),
                    NumeroIdentificacion = table.Column<string>(type: "text", nullable: false),
                    Direccion = table.Column<string>(type: "text", nullable: true),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    IdRecepcion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoRecibido = table.Column<string>(type: "text", nullable: false),
                    Peso = table.Column<double>(type: "double precision", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaCaptura = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LugarOrigen = table.Column<string>(type: "text", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    TipoProveedor = table.Column<string>(type: "text", nullable: false),
                    NombreProveedor = table.Column<string>(type: "text", nullable: false),
                    UbicacionProveedor = table.Column<string>(type: "text", nullable: true),
                    TelefonoProveedor = table.Column<string>(type: "text", nullable: true),
                    EmailProveedor = table.Column<string>(type: "text", nullable: true),
                    CodigoQR = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.IdRecepcion);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Almacenamientos",
                columns: table => new
                {
                    IdAlmacenamiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRecepcion = table.Column<int>(type: "integer", nullable: false),
                    RecepcionIdRecepcion = table.Column<int>(type: "integer", nullable: true),
                    TipoAlmacenamiento = table.Column<string>(type: "text", nullable: false),
                    Ubicacion = table.Column<string>(type: "text", nullable: false),
                    FechaAlmacenamiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Condiciones = table.Column<string>(type: "text", nullable: true),
                    Responsable = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Almacenamientos", x => x.IdAlmacenamiento);
                    table.ForeignKey(
                        name: "FK_Almacenamientos_Productos_RecepcionIdRecepcion",
                        column: x => x.RecepcionIdRecepcion,
                        principalTable: "Productos",
                        principalColumn: "IdRecepcion");
                });

            migrationBuilder.CreateTable(
                name: "Calidades",
                columns: table => new
                {
                    IdCalidad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRecepcion = table.Column<int>(type: "integer", nullable: false),
                    RecepcionIdRecepcion = table.Column<int>(type: "integer", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Olor = table.Column<string>(type: "text", nullable: false),
                    Textura = table.Column<string>(type: "text", nullable: false),
                    Resultado = table.Column<string>(type: "text", nullable: false),
                    FechaEvaluacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calidades", x => x.IdCalidad);
                    table.ForeignKey(
                        name: "FK_Calidades_Productos_RecepcionIdRecepcion",
                        column: x => x.RecepcionIdRecepcion,
                        principalTable: "Productos",
                        principalColumn: "IdRecepcion");
                });

            migrationBuilder.CreateTable(
                name: "Conservaciones",
                columns: table => new
                {
                    IdConservacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRecepcion = table.Column<int>(type: "integer", nullable: false),
                    RecepcionIdRecepcion = table.Column<int>(type: "integer", nullable: true),
                    MetodoConservacion = table.Column<string>(type: "text", nullable: false),
                    FechaConservacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DuracionEstimada = table.Column<int>(type: "integer", nullable: false),
                    Temperatura = table.Column<double>(type: "double precision", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conservaciones", x => x.IdConservacion);
                    table.ForeignKey(
                        name: "FK_Conservaciones_Productos_RecepcionIdRecepcion",
                        column: x => x.RecepcionIdRecepcion,
                        principalTable: "Productos",
                        principalColumn: "IdRecepcion");
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    IdVenta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRecepcion = table.Column<int>(type: "integer", nullable: false),
                    RecepcionIdRecepcion = table.Column<int>(type: "integer", nullable: true),
                    IdCliente = table.Column<int>(type: "integer", nullable: false),
                    ClienteIdCliente = table.Column<int>(type: "integer", nullable: true),
                    FechaVenta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CantidadVendida = table.Column<double>(type: "double precision", nullable: false),
                    PrecioUnitario = table.Column<double>(type: "double precision", nullable: false),
                    PrecioTotal = table.Column<double>(type: "double precision", nullable: false),
                    MetodoPago = table.Column<string>(type: "text", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.IdVenta);
                    table.ForeignKey(
                        name: "FK_Ventas_Clientes_ClienteIdCliente",
                        column: x => x.ClienteIdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente");
                    table.ForeignKey(
                        name: "FK_Ventas_Productos_RecepcionIdRecepcion",
                        column: x => x.RecepcionIdRecepcion,
                        principalTable: "Productos",
                        principalColumn: "IdRecepcion");
                });

            migrationBuilder.CreateTable(
                name: "Inventarios",
                columns: table => new
                {
                    IdInventario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAlmacenamiento = table.Column<int>(type: "integer", nullable: false),
                    AlmacenamientoIdAlmacenamiento = table.Column<int>(type: "integer", nullable: true),
                    CantidadDisponible = table.Column<double>(type: "double precision", nullable: false),
                    UnidadMedida = table.Column<string>(type: "text", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstadoStock = table.Column<string>(type: "text", nullable: false),
                    Ubicacion = table.Column<string>(type: "text", nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventarios", x => x.IdInventario);
                    table.ForeignKey(
                        name: "FK_Inventarios_Almacenamientos_AlmacenamientoIdAlmacenamiento",
                        column: x => x.AlmacenamientoIdAlmacenamiento,
                        principalTable: "Almacenamientos",
                        principalColumn: "IdAlmacenamiento");
                });

            migrationBuilder.CreateTable(
                name: "Comercializaciones",
                columns: table => new
                {
                    IdComercializacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdVenta = table.Column<int>(type: "integer", nullable: false),
                    VentaIdVenta = table.Column<int>(type: "integer", nullable: true),
                    CanalVenta = table.Column<string>(type: "text", nullable: false),
                    Destino = table.Column<string>(type: "text", nullable: false),
                    FechaComercializacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comercializaciones", x => x.IdComercializacion);
                    table.ForeignKey(
                        name: "FK_Comercializaciones_Ventas_VentaIdVenta",
                        column: x => x.VentaIdVenta,
                        principalTable: "Ventas",
                        principalColumn: "IdVenta");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Almacenamientos_RecepcionIdRecepcion",
                table: "Almacenamientos",
                column: "RecepcionIdRecepcion");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calidades_RecepcionIdRecepcion",
                table: "Calidades",
                column: "RecepcionIdRecepcion");

            migrationBuilder.CreateIndex(
                name: "IX_Comercializaciones_VentaIdVenta",
                table: "Comercializaciones",
                column: "VentaIdVenta");

            migrationBuilder.CreateIndex(
                name: "IX_Conservaciones_RecepcionIdRecepcion",
                table: "Conservaciones",
                column: "RecepcionIdRecepcion");

            migrationBuilder.CreateIndex(
                name: "IX_Inventarios_AlmacenamientoIdAlmacenamiento",
                table: "Inventarios",
                column: "AlmacenamientoIdAlmacenamiento");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_ClienteIdCliente",
                table: "Ventas",
                column: "ClienteIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_RecepcionIdRecepcion",
                table: "Ventas",
                column: "RecepcionIdRecepcion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Calidades");

            migrationBuilder.DropTable(
                name: "Comercializaciones");

            migrationBuilder.DropTable(
                name: "Conservaciones");

            migrationBuilder.DropTable(
                name: "Inventarios");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "Almacenamientos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Productos");
        }
    }
}
