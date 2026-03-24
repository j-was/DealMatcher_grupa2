using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealMatcher.Backend.Infrastructure.Migrations;

/// <inheritdoc />
public partial class OfferAggregateV1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Categorys",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categorys", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Rating = table.Column<float>(type: "real", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Offers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Price = table.Column<double>(type: "float", nullable: false),
                ImageUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SellerId = table.Column<int>(type: "int", nullable: false),
                Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CategoryId = table.Column<int>(type: "int", nullable: false),
                Availability = table.Column<int>(type: "int", nullable: false),
                Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Offers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Offers_Users_SellerId",
                    column: x => x.SellerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "OfferProperties",
            columns: table => new
            {
                OfferId = table.Column<int>(type: "int", nullable: false),
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OfferProperties", x => new { x.OfferId, x.Id });
                table.ForeignKey(
                    name: "FK_OfferProperties_Offers_OfferId",
                    column: x => x.OfferId,
                    principalTable: "Offers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_OfferProperties_Name",
            table: "OfferProperties",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_Offers_SellerId",
            table: "Offers",
            column: "SellerId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Categorys");

        migrationBuilder.DropTable(
            name: "OfferProperties");

        migrationBuilder.DropTable(
            name: "Offers");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
