using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _2024FinalYearProject.Migrations
{
    /// <inheritdoc />
    public partial class updatedAdvice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "AdviceText",
                table: "Advices",
                newName: "clientName");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Advices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Advices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Expenses",
                table: "Advices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Income",
                table: "Advices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Advices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "advisorName",
                table: "Advices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2990b3ed-6d0a-4a72-9598-864cf7ad6acc", null, "Consultant", "CONSULTANT" },
                    { "462d88c6-abc9-4d87-8b4f-5e6040d8f864", null, "User", "USER" },
                    { "5cac391b-8b40-417f-b409-d2135b9e05e6", null, "Admin", "ADMIN" },
                    { "c993cbe3-e5cf-404f-9e5c-10b9446283c1", null, "Advisor", "ADVISOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2990b3ed-6d0a-4a72-9598-864cf7ad6acc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "462d88c6-abc9-4d87-8b4f-5e6040d8f864");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5cac391b-8b40-417f-b409-d2135b9e05e6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c993cbe3-e5cf-404f-9e5c-10b9446283c1");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Advices");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Advices");

            migrationBuilder.DropColumn(
                name: "Expenses",
                table: "Advices");

            migrationBuilder.DropColumn(
                name: "Income",
                table: "Advices");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Advices");

            migrationBuilder.DropColumn(
                name: "advisorName",
                table: "Advices");

            migrationBuilder.RenameColumn(
                name: "clientName",
                table: "Advices",
                newName: "AdviceText");

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
    }
}
