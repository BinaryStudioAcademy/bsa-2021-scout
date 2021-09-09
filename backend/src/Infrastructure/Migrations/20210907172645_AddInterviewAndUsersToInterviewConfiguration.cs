using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddInterviewAndUsersToInterviewConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Vacancies_VacancyId",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersToInterviews_Interviews_InterviewId",
                table: "UsersToInterviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersToInterviews_Users_UserId",
                table: "UsersToInterviews");

            migrationBuilder.AddForeignKey(
                name: "interview__vacancy_FK",
                table: "Interviews",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "user_interview__interview_FK",
                table: "UsersToInterviews",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "user_interview__user_FK",
                table: "UsersToInterviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "interview__vacancy_FK",
                table: "Interviews");

            migrationBuilder.DropForeignKey(
                name: "user_interview__interview_FK",
                table: "UsersToInterviews");

            migrationBuilder.DropForeignKey(
                name: "user_interview__user_FK",
                table: "UsersToInterviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Vacancies_VacancyId",
                table: "Interviews",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersToInterviews_Interviews_InterviewId",
                table: "UsersToInterviews",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersToInterviews_Users_UserId",
                table: "UsersToInterviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
