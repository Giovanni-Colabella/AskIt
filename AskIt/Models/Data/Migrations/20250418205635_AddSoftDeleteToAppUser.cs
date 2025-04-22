using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AskIt.Models.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: nameof(Models.Enums.AccountStatus.Attivo));

            migrationBuilder.Sql($"UPDATE AspNetUsers SET Status = '{nameof(Models.Enums.AccountStatus.Attivo)}'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");
        }
    }
}
