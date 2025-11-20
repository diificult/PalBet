using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalBet.Migrations
{
    /// <inheritdoc />
    public partial class OutcomeChoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserWinner",
                table: "bets");

            // Use explicit, schema-qualified renames guarded by existence checks
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.columns c
    JOIN sys.objects o ON c.object_id = o.object_id
    WHERE o.name = 'Groups' AND SCHEMA_NAME(o.schema_id) = 'dbo' AND c.name = 'Text'
)
BEGIN
    EXEC sp_rename N'[dbo].[Groups].[Text]', N'Name', N'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.columns c
    JOIN sys.objects o ON c.object_id = o.object_id
    WHERE o.name = 'AspNetUserTokens' AND SCHEMA_NAME(o.schema_id) = 'dbo' AND c.name = 'Text'
)
BEGIN
    EXEC sp_rename N'[dbo].[AspNetUserTokens].[Text]', N'Name', N'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.columns c
    JOIN sys.objects o ON c.object_id = o.object_id
    WHERE o.name = 'AspNetRoles' AND SCHEMA_NAME(o.schema_id) = 'dbo' AND c.name = 'Text'
)
BEGIN
    EXEC sp_rename N'[dbo].[AspNetRoles].[Text]', N'Name', N'COLUMN';
END
");

            migrationBuilder.AddColumn<bool>(
                name: "IsWinner",
                table: "participants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SelectedChoiceId",
                table: "participants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowMultipleWinners",
                table: "bets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BurnStakeOnNoWnner",
                table: "bets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OutcomeChoice",
                table: "bets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BetChoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetChoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetChoice_bets_BetId",
                        column: x => x.BetId,
                        principalTable: "bets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "Name",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "Name",
                value: "User");

            migrationBuilder.CreateIndex(
                name: "IX_participants_SelectedChoiceId",
                table: "participants",
                column: "SelectedChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BetChoice_BetId",
                table: "BetChoice",
                column: "BetId");

            migrationBuilder.AddForeignKey(
                name: "FK_participants_BetChoice_SelectedChoiceId",
                table: "participants",
                column: "SelectedChoiceId",
                principalTable: "BetChoice",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_participants_BetChoice_SelectedChoiceId",
                table: "participants");

            migrationBuilder.DropTable(
                name: "BetChoice");

            migrationBuilder.DropIndex(
                name: "IX_participants_SelectedChoiceId",
                table: "participants");

            migrationBuilder.DropColumn(
                name: "IsWinner",
                table: "participants");

            migrationBuilder.DropColumn(
                name: "SelectedChoiceId",
                table: "participants");

            migrationBuilder.DropColumn(
                name: "AllowMultipleWinners",
                table: "bets");

            migrationBuilder.DropColumn(
                name: "BurnStakeOnNoWnner",
                table: "bets");

            migrationBuilder.DropColumn(
                name: "OutcomeChoice",
                table: "bets");

            // Reverse renames (guarded)
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.columns c
    JOIN sys.objects o ON c.object_id = o.object_id
    WHERE o.name = 'Groups' AND SCHEMA_NAME(o.schema_id) = 'dbo' AND c.name = 'Name'
)
BEGIN
    EXEC sp_rename N'[dbo].[Groups].[Name]', N'Text', N'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.columns c
    JOIN sys.objects o ON c.object_id = o.object_id
    WHERE o.name = 'AspNetUserTokens' AND SCHEMA_NAME(o.schema_id) = 'dbo' AND c.name = 'Name'
)
BEGIN
    EXEC sp_rename N'[dbo].[AspNetUserTokens].[Name]', N'Text', N'COLUMN';
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.columns c
    JOIN sys.objects o ON c.object_id = o.object_id
    WHERE o.name = 'AspNetRoles' AND SCHEMA_NAME(o.schema_id) = 'dbo' AND c.name = 'Name'
)
BEGIN
    EXEC sp_rename N'[dbo].[AspNetRoles].[Name]', N'Text', N'COLUMN';
END
");

            migrationBuilder.AddColumn<string>(
                name: "UserWinner",
                table: "bets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "Text",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "Text",
                value: null);
        }
    }
}