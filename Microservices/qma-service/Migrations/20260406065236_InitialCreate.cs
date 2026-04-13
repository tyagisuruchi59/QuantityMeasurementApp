using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qma_service.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThisValue = table.Column<double>(type: "float", nullable: false),
                    ThisUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ThisMeasurementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThatValue = table.Column<double>(type: "float", nullable: false),
                    ThatUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ThatMeasurementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResultString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultValue = table.Column<double>(type: "float", nullable: false),
                    ResultUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultMeasurementType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsError = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");
        }
    }
}
