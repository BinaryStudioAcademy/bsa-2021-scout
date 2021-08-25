using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddApplicantFieldsAndRemoveMiddlename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "Applicants",
                newName: "Skills");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Pools",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExperienceDescription",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pools_CreatedById",
                table: "Pools",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Pools_Users_CreatedById",
                table: "Pools",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pools_Users_CreatedById",
                table: "Pools");

            migrationBuilder.DropIndex(
                name: "IX_Pools_CreatedById",
                table: "Pools");

            migrationBuilder.DropColumn(
                name: "ExperienceDescription",
                table: "Applicants");

            migrationBuilder.RenameColumn(
                name: "Skills",
                table: "Applicants",
                newName: "MiddleName");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Pools",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
