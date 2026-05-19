using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealMatcher.Backend.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ActivityRecords : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ActivityRecords",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                OfferId = table.Column<int>(type: "int", nullable: true),
                Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ActivityRecords", x => x.Id);
                table.ForeignKey(
                    name: "FK_ActivityRecords_Offers_OfferId",
                    column: x => x.OfferId,
                    principalTable: "Offers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ActivityRecords_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ActivityRecordDetails",
            columns: table => new
            {
                ActivityId = table.Column<int>(type: "int", nullable: false),
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ActivityRecordDetails", x => new { x.ActivityId, x.Id });
                table.ForeignKey(
                    name: "FK_ActivityRecordDetails_ActivityRecords_ActivityId",
                    column: x => x.ActivityId,
                    principalTable: "ActivityRecords",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ActivityRecordDetails_Name",
            table: "ActivityRecordDetails",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_ActivityRecords_OfferId",
            table: "ActivityRecords",
            column: "OfferId");

        migrationBuilder.CreateIndex(
            name: "IX_ActivityRecords_UserId",
            table: "ActivityRecords",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ActivityRecordDetails");

        migrationBuilder.DropTable(
            name: "ActivityRecords");
    }
}
