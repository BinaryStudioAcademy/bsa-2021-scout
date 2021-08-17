using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddVacancy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tier",
                table: "Vacancies",
                newName: "TierTo");

            migrationBuilder.AddColumn<bool>(
                name: "IsHot",
                table: "Vacancies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SalaryFrom",
                table: "Vacancies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalaryTo",
                table: "Vacancies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TierFrom",
                table: "Vacancies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHot",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "SalaryFrom",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "SalaryTo",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "TierFrom",
                table: "Vacancies");

            migrationBuilder.RenameColumn(
                name: "TierTo",
                table: "Vacancies",
                newName: "Tier");
        }
    }
}
