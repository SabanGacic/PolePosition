using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PolePosition.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin", "ADMIN" },
                    { "2", null, "Driver", "DRIVER" },
                    { "3", null, "Instructor", "INSTRUCTOR" }
                });

            migrationBuilder.InsertData(
                table: "EquipmentTypes",
                columns: new[] { "Id", "AdditionalPrice", "Name" },
                values: new object[,]
                {
                    { 1, 0m, "Kaciga" },
                    { 2, 5m, "Kombinezon" }
                });

            migrationBuilder.InsertData(
                table: "SessionTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Opusten krug bez pritiska vremena", "Slobodna voznja" },
                    { 2, "Takmicarska sesija sa rang listom", "Trka" },
                    { 3, "Sesija sa instruktorom za pocetnike", "Obuka" }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "Description", "LengthMeters", "Location", "Name" },
                values: new object[] { 1, null, 850.0, "Bugojno", "Kart Arena Bugojno" });

            migrationBuilder.InsertData(
                table: "Karts",
                columns: new[] { "Id", "Code", "Model", "Status", "TrackId" },
                values: new object[,]
                {
                    { 1, "K-01", "Sodikart RT8", 0, 1 },
                    { 2, "K-02", "Sodikart RT8", 0, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EquipmentTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Karts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Karts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SessionTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SessionTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SessionTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
