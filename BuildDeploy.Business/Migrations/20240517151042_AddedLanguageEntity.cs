using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildDeploy.Business.Migrations
{
    /// <inheritdoc />
    public partial class AddedLanguageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ExecutableName = table.Column<string>(type: "TEXT", nullable: true),
                    BuildCommand = table.Column<string>(type: "TEXT", nullable: true),
                    ArgumentsJson = table.Column<string>(type: "TEXT", nullable: true),
                    IsJavascriptFramework = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LanguageId",
                table: "Projects",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Language_LanguageId",
                table: "Projects",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Language_LanguageId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropIndex(
                name: "IX_Projects_LanguageId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Projects");
        }
    }
}
