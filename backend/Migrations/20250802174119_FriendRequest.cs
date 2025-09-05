using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalBet.Migrations
{
    /// <inheritdoc />
    public partial class FriendRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "friendships",
                columns: table => new
                {
                    RequesterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequesteeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    FriendshipTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friendships", x => new { x.RequesterId, x.RequesteeId });
                    table.ForeignKey(
                        name: "FK_friendships_AspNetUsers_RequesteeId",
                        column: x => x.RequesteeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_friendships_AspNetUsers_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_friendships_RequesteeId",
                table: "friendships",
                column: "RequesteeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "friendships");
        }
    }
}
