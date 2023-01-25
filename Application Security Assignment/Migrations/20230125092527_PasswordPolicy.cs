using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationSecurityAssignment.Migrations
{
    /// <inheritdoc />
    public partial class PasswordPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PasswordCreation",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousPasswordHash",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordCreation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PreviousPasswordHash",
                table: "AspNetUsers");
        }
    }
}
