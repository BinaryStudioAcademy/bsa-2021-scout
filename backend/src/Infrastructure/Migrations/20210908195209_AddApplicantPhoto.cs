using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddApplicantPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoFileInfoId",
                table: "Applicants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_PhotoFileInfoId",
                table: "Applicants",
                column: "PhotoFileInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_FileInfos_PhotoFileInfoId",
                table: "Applicants",
                column: "PhotoFileInfoId",
                principalTable: "FileInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_FileInfos_PhotoFileInfoId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_PhotoFileInfoId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "PhotoFileInfoId",
                table: "Applicants");
        }
    }
}
