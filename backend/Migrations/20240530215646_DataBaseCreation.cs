using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "041965ea-e7ec-4a2b-874b-a8d61f3239ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "617c881d-a226-4116-a2ef-e79fcd6896eb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "452973aa-fe27-4fba-93f2-ed209d4e56b0", null, "Usuario", "USUARIO" },
                    { "6a730649-8ddd-4c2c-86d4-5a30e34cf60f", null, "Administrador", "ADMINISTRADOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "452973aa-fe27-4fba-93f2-ed209d4e56b0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a730649-8ddd-4c2c-86d4-5a30e34cf60f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "041965ea-e7ec-4a2b-874b-a8d61f3239ad", null, "Usuario", "USUARIO" },
                    { "617c881d-a226-4116-a2ef-e79fcd6896eb", null, "Administrador", "ADMINISTRADOR" }
                });
        }
    }
}
