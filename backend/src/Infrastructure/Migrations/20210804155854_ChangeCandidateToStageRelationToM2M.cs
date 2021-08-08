using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ChangeCandidateToStageRelationToM2M : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "candidate_stage_FK",
                table: "VacancyCandidates");

            migrationBuilder.CreateTable(
                name: "CandidateToStages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CandidateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRemoved = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateToStages", x => x.Id);
                    table.ForeignKey(
                        name: "candidate_to_stage_candidate_FK",
                        column: x => x.CandidateId,
                        principalTable: "VacancyCandidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "candidate_to_stage_stage_FK",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateToStages_CandidateId",
                table: "CandidateToStages",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateToStages_StageId",
                table: "CandidateToStages",
                column: "StageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StageId",
                table: "VacancyCandidates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.DropTable(
                name: "CandidateToStages");

            migrationBuilder.AddForeignKey(
                name: "candidate_stage_FK",
                table: "VacancyCandidates",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
