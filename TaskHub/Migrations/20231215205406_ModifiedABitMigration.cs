using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskHub.Migrations
{
    public partial class ModifiedABitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Proiecte_ProiectId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "ProiectId",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ContinutMedia",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Proiecte_ProiectId",
                table: "Tasks",
                column: "ProiectId",
                principalTable: "Proiecte",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Proiecte_ProiectId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "ProiectId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContinutMedia",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Proiecte_ProiectId",
                table: "Tasks",
                column: "ProiectId",
                principalTable: "Proiecte",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
