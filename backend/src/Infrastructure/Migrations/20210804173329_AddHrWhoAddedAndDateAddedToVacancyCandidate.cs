using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddHrWhoAddedAndDateAddedToVacancyCandidate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "VacancyCandidates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "HrWhoAddedId",
                table: "VacancyCandidates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VacancyCandidates_HrWhoAddedId",
                table: "VacancyCandidates",
                column: "HrWhoAddedId");

            migrationBuilder.AddForeignKey(
                name: "candidate_hr_who_added_FK",
                table: "VacancyCandidates",
                column: "HrWhoAddedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "candidate_hr_who_added_FK",
                table: "VacancyCandidates");

            migrationBuilder.DropIndex(
                name: "IX_VacancyCandidates_HrWhoAddedId",
                table: "VacancyCandidates");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "VacancyCandidates");

            migrationBuilder.DropColumn(
                name: "HrWhoAddedId",
                table: "VacancyCandidates");
        }
    }
}
