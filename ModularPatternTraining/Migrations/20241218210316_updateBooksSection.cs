using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModularPatternTraining.Migrations
{
    /// <inheritdoc />
    public partial class updateBooksSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Books");
        }
    }
}
