using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddIsSelfAppliedAndIsViewed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelfApplied",
                table: "VacancyCandidates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsViewed",
                table: "VacancyCandidates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSelfApplied",
                table: "Applicants",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelfApplied",
                table: "VacancyCandidates");

            migrationBuilder.DropColumn(
                name: "IsViewed",
                table: "VacancyCandidates");

            migrationBuilder.DropColumn(
                name: "IsSelfApplied",
                table: "Applicants");
        }
    }
}
