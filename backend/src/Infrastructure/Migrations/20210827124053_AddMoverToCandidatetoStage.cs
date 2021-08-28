using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddMoverToCandidatetoStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MoverId",
                table: "CandidateToStages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateToStages_MoverId",
                table: "CandidateToStages",
                column: "MoverId");

            migrationBuilder.AddForeignKey(
                name: "candidate_to_stage_mover_FK",
                table: "CandidateToStages",
                column: "MoverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "candidate_to_stage_mover_FK",
                table: "CandidateToStages");

            migrationBuilder.DropIndex(
                name: "IX_CandidateToStages_MoverId",
                table: "CandidateToStages");

            migrationBuilder.DropColumn(
                name: "MoverId",
                table: "CandidateToStages");
        }
    }
}
