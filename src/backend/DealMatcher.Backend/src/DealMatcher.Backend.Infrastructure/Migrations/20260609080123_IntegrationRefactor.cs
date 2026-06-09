using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealMatcher.Backend.Infrastructure.Migrations;

/// <inheritdoc />
public partial class IntegrationRefactor : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_OfferProperties_Name",
            table: "OfferProperties");

        migrationBuilder.DropColumn(
            name: "Name",
            table: "OfferProperties");

        migrationBuilder.AddColumn<string>(
            name: "PropertyId",
            table: "OfferProperties",
            type: "nvarchar(450)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "IX_OfferProperties_PropertyId",
            table: "OfferProperties",
            column: "PropertyId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_OfferProperties_PropertyId",
            table: "OfferProperties");

        migrationBuilder.DropColumn(
            name: "PropertyId",
            table: "OfferProperties");

        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "OfferProperties",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "IX_OfferProperties_Name",
            table: "OfferProperties",
            column: "Name");
    }
}
