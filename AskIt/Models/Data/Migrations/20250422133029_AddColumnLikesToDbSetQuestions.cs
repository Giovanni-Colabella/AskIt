using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AskIt.Models.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnLikesToDbSetQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Questions");
        }
    }
}
