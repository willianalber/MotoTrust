using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoTrust.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryPersons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Identificador = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CNPJ = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumeroCNH = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TipoCNH = table.Column<string>(type: "text", nullable: false),
                    ImagemCNH = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPersons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MotorcycleNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MotorcycleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Identificador = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    Modelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Placa = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TipoNotificacao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Mensagem = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DataEvento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorcycleNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motorcycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LicensePlate = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DailyRate = table.Column<decimal>(type: "numeric", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    EngineCapacity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorcycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntregadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    MotoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataTermino = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataPrevisaoTermino = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataDevolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValorDiaria = table.Column<decimal>(type: "numeric", nullable: false),
                    Plano = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentals_DeliveryPersons_EntregadorId",
                        column: x => x.EntregadorId,
                        principalTable: "DeliveryPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rentals_Motorcycles_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Motorcycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPersons_CNPJ",
                table: "DeliveryPersons",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPersons_Identificador",
                table: "DeliveryPersons",
                column: "Identificador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPersons_NumeroCNH",
                table: "DeliveryPersons",
                column: "NumeroCNH",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleNotifications_Ano",
                table: "MotorcycleNotifications",
                column: "Ano");

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleNotifications_DataEvento",
                table: "MotorcycleNotifications",
                column: "DataEvento");

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleNotifications_TipoNotificacao",
                table: "MotorcycleNotifications",
                column: "TipoNotificacao");

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_LicensePlate",
                table: "Motorcycles",
                column: "LicensePlate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_EntregadorId",
                table: "Rentals",
                column: "EntregadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_MotoId",
                table: "Rentals",
                column: "MotoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotorcycleNotifications");

            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "DeliveryPersons");

            migrationBuilder.DropTable(
                name: "Motorcycles");
        }
    }
}
