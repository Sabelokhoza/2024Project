using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _2024FinalYearProject.Migrations
{
    /// <inheritdoc />
    public partial class updateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Reports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "consultantId",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7a8a6526-c434-4318-a17a-e49022fd9b57", null, "Consultant", "CONSULTANT" },
                    { "caa67e53-0dda-445f-8ce1-b6d5b32452b8", null, "Advisor", "ADVISOR" },
                    { "df7c52ea-24b1-4f12-9bda-a743b12d1767", null, "Admin", "ADMIN" },
                    { "f80ba6e2-b0a2-4f42-8e73-1d80ad5ed9cf", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a8a6526-c434-4318-a17a-e49022fd9b57");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "caa67e53-0dda-445f-8ce1-b6d5b32452b8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df7c52ea-24b1-4f12-9bda-a743b12d1767");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f80ba6e2-b0a2-4f42-8e73-1d80ad5ed9cf");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "consultantId",
                table: "Reports");

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
    }
}
