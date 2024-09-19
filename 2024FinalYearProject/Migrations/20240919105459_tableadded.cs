using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _2024FinalYearProject.Migrations
{
    /// <inheritdoc />
    public partial class tableadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74ada071-f031-456e-9542-48b3482783e4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89db8047-c66c-43a6-b65f-06431c402993");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c715a60-a494-4a44-bd7c-aeb0d8fa57ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ebebf595-4011-488e-8669-e8f3cc2166b9");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4b372eca-6b8b-48fd-8179-4fe8c33881e0", null, "User", "USER" },
                    { "5650b64a-c2e8-48e8-aacb-7ef14832c7fc", null, "Advisor", "ADVISOR" },
                    { "7823f160-12ea-49fb-afe8-981eebc18551", null, "Admin", "ADMIN" },
                    { "a60aa8f6-5f72-4ab8-89f6-fb1ce3c1da1b", null, "Consultant", "CONSULTANT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4b372eca-6b8b-48fd-8179-4fe8c33881e0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5650b64a-c2e8-48e8-aacb-7ef14832c7fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7823f160-12ea-49fb-afe8-981eebc18551");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a60aa8f6-5f72-4ab8-89f6-fb1ce3c1da1b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "74ada071-f031-456e-9542-48b3482783e4", null, "User", "USER" },
                    { "89db8047-c66c-43a6-b65f-06431c402993", null, "Admin", "ADMIN" },
                    { "9c715a60-a494-4a44-bd7c-aeb0d8fa57ae", null, "Consultant", "CONSULTANT" },
                    { "ebebf595-4011-488e-8669-e8f3cc2166b9", null, "Advisor", "ADVISOR" }
                });
        }
    }
}
