using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddedNoteFieldMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_CandidateComments_NoteId",
                table: "Interviews");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_NoteId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Interviews");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Interviews",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Interviews");

            migrationBuilder.AddColumn<string>(
                name: "NoteId",
                table: "Interviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_NoteId",
                table: "Interviews",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_CandidateComments_NoteId",
                table: "Interviews",
                column: "NoteId",
                principalTable: "CandidateComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
