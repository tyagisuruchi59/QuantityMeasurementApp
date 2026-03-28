using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementAppRepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndQuantityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuantityMeasurements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    this_value = table.Column<double>(type: "float", nullable: false),
                    this_unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    this_measurement_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    that_value = table.Column<double>(type: "float", nullable: false),
                    that_unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    that_measurement_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    result_string = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    result_value = table.Column<double>(type: "float", nullable: false),
                    result_unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    result_measurement_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    error_message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_error = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantityMeasurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    salt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "User"),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_login = table.Column<DateTime>(type: "datetime2", nullable: true),
                    refresh_token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refresh_token_expiry = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QM_CreatedAt",
                table: "QuantityMeasurements",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_QM_IsError",
                table: "QuantityMeasurements",
                column: "is_error");

            migrationBuilder.CreateIndex(
                name: "IX_QM_MeasurementType",
                table: "QuantityMeasurements",
                column: "this_measurement_type");

            migrationBuilder.CreateIndex(
                name: "IX_QM_Operation",
                table: "QuantityMeasurements",
                column: "operation");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuantityMeasurements");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
