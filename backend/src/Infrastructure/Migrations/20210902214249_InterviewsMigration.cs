using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class InterviewsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Interviews_InterviewId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_InterviewId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InterviewId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InterviewId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_InterviewId",
                table: "Users",
                column: "InterviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Interviews_InterviewId",
                table: "Users",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
