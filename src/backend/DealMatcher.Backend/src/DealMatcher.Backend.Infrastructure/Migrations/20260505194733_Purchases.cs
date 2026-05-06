using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealMatcher.Backend.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Purchases : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DeliveryMethods",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StringId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                EstimatedDays = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DeliveryMethods", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PaymentMethods",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StringId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Icon = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PaymentMethods", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DeliveryMethods_StringId",
            table: "DeliveryMethods",
            column: "StringId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PaymentMethods_StringId",
            table: "PaymentMethods",
            column: "StringId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DeliveryMethods");

        migrationBuilder.DropTable(
            name: "PaymentMethods");
    }
}
