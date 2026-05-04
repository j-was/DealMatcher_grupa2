using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealMatcher.Backend.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddConversations : Migration
{
    private static readonly string[] OfferBuyerSellerColumns =
    [
        "OfferId", "BuyerId", "SellerId"
    ];
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Conversations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                OfferId = table.Column<int>(type: "int", nullable: false),
                BuyerId = table.Column<int>(type: "int", nullable: false),
                SellerId = table.Column<int>(type: "int", nullable: false),
                LastMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastMessageAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UnreadCount = table.Column<int>(type: "int", nullable: false),
                Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Conversations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Conversations_Offers_OfferId",
                    column: x => x.OfferId,
                    principalTable: "Offers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Conversations_Users_BuyerId",
                    column: x => x.BuyerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Conversations_Users_SellerId",
                    column: x => x.SellerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Messages",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ConversationId = table.Column<int>(type: "int", nullable: false),
                SenderId = table.Column<int>(type: "int", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Messages", x => x.Id);
                table.ForeignKey(
                    name: "FK_Messages_Conversations_ConversationId",
                    column: x => x.ConversationId,
                    principalTable: "Conversations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Messages_Users_SenderId",
                    column: x => x.SenderId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Conversations_BuyerId",
            table: "Conversations",
            column: "BuyerId");

        migrationBuilder.CreateIndex(
            name: "IX_Conversations_OfferId",
            table: "Conversations",
            column: "OfferId");

        migrationBuilder.CreateIndex(
            name: "IX_Conversations_OfferId_BuyerId_SellerId",
            table: "Conversations",
            columns: OfferBuyerSellerColumns,
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Conversations_SellerId",
            table: "Conversations",
            column: "SellerId");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_ConversationId",
            table: "Messages",
            column: "ConversationId");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_CreatedAt",
            table: "Messages",
            column: "CreatedAt");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_SenderId",
            table: "Messages",
            column: "SenderId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Messages");

        migrationBuilder.DropTable(
            name: "Conversations");
    }
}
