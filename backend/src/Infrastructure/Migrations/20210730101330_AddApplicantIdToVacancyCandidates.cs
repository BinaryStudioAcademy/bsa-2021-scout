using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddApplicantIdToVacancyCandidates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "candidate_applicant_FK",
                table: "VacancyCandidates");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyCandidates_ApplicantId",
                table: "VacancyCandidates",
                column: "ApplicantId");

            migrationBuilder.AddForeignKey(
                name: "candidate_applicant_FK",
                table: "VacancyCandidates",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "candidate_applicant_FK",
                table: "VacancyCandidates");

            migrationBuilder.DropIndex(
                name: "IX_VacancyCandidates_ApplicantId",
                table: "VacancyCandidates");

            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "VacancyCandidates");

            migrationBuilder.AddForeignKey(
                name: "candidate_applicant_FK",
                table: "VacancyCandidates",
                column: "Id",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
