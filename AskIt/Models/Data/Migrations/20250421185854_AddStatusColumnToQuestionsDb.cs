using AskIt.Models.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AskIt.Models.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusColumnToQuestionsDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: nameof(QuestionStatus.Open),
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql($"UPDATE Questions SET Status = '{QuestionStatus.Open}'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Open");
        }
    }
}
