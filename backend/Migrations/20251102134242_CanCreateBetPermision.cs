using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalBet.Migrations
{
    /// <inheritdoc />
    public partial class CanCreateBetPermision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanCreateBet",
                table: "UserGroup",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanCreateBet",
                table: "UserGroup");
        }
    }
}
