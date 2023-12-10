using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskHub.Migrations
{
    public partial class AddedRolesAndIdentityToUtilizator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "Utilizatori",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Utilizatori",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilizatori_RoleId",
                table: "Utilizatori",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizatori_UserId",
                table: "Utilizatori",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizatori_AspNetRoles_RoleId",
                table: "Utilizatori",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizatori_AspNetUsers_UserId",
                table: "Utilizatori",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizatori_AspNetRoles_RoleId",
                table: "Utilizatori");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizatori_AspNetUsers_UserId",
                table: "Utilizatori");

            migrationBuilder.DropIndex(
                name: "IX_Utilizatori_RoleId",
                table: "Utilizatori");

            migrationBuilder.DropIndex(
                name: "IX_Utilizatori_UserId",
                table: "Utilizatori");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Utilizatori");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Utilizatori");
        }
    }
}
