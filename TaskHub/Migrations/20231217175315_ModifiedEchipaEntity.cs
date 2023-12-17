using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskHub.Migrations
{
    public partial class ModifiedEchipaEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RolInProiect",
                table: "Echipe");

            migrationBuilder.AddColumn<string>(
                name: "RolInProiectId",
                table: "Echipe",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Echipe_RolInProiectId",
                table: "Echipe",
                column: "RolInProiectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Echipe_AspNetRoles_RolInProiectId",
                table: "Echipe",
                column: "RolInProiectId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Echipe_AspNetRoles_RolInProiectId",
                table: "Echipe");

            migrationBuilder.DropIndex(
                name: "IX_Echipe_RolInProiectId",
                table: "Echipe");

            migrationBuilder.DropColumn(
                name: "RolInProiectId",
                table: "Echipe");

            migrationBuilder.AddColumn<string>(
                name: "RolInProiect",
                table: "Echipe",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
