using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalBet.Migrations
{
    /// <inheritdoc />
    public partial class BetStakeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_AspNetUsers_notifyeeId",
                table: "notification");

            migrationBuilder.RenameColumn(
                name: "notifyeeId",
                table: "notification",
                newName: "NotifyeeId");

            migrationBuilder.RenameColumn(
                name: "notificationType",
                table: "notification",
                newName: "NotificationType");

            migrationBuilder.RenameColumn(
                name: "isRead",
                table: "notification",
                newName: "IsRead");

            migrationBuilder.RenameColumn(
                name: "isCompleted",
                table: "notification",
                newName: "IsCompleted");

            migrationBuilder.RenameIndex(
                name: "IX_notification_notifyeeId",
                table: "notification",
                newName: "IX_notification_NotifyeeId");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "bets",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "BetStake",
                table: "bets",
                newName: "BetStakeType");

            migrationBuilder.AddColumn<int>(
                name: "Coins",
                table: "bets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserInput",
                table: "bets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_AspNetUsers_NotifyeeId",
                table: "notification",
                column: "NotifyeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_AspNetUsers_NotifyeeId",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "Coins",
                table: "bets");

            migrationBuilder.DropColumn(
                name: "UserInput",
                table: "bets");

            migrationBuilder.RenameColumn(
                name: "NotifyeeId",
                table: "notification",
                newName: "notifyeeId");

            migrationBuilder.RenameColumn(
                name: "NotificationType",
                table: "notification",
                newName: "notificationType");

            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "notification",
                newName: "isRead");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "notification",
                newName: "isCompleted");

            migrationBuilder.RenameIndex(
                name: "IX_notification_NotifyeeId",
                table: "notification",
                newName: "IX_notification_notifyeeId");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "bets",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "BetStakeType",
                table: "bets",
                newName: "BetStake");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_AspNetUsers_notifyeeId",
                table: "notification",
                column: "notifyeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
