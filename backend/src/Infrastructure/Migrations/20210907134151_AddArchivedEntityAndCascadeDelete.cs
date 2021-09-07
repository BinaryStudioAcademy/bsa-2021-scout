using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddArchivedEntityAndCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "action_stage_FK",
                table: "Actions");

            migrationBuilder.DropForeignKey(
                name: "apply_token__vacancy_FK",
                table: "ApplyTokens");

            migrationBuilder.DropForeignKey(
                name: "candidate_comment_candidate_FK",
                table: "CandidateComments");

            migrationBuilder.DropForeignKey(
                name: "candidate_comment_stage_FK",
                table: "CandidateComments");

            migrationBuilder.DropForeignKey(
                name: "candidate_review_candidate_FK",
                table: "CandidateReviews");

            migrationBuilder.DropForeignKey(
                name: "candidate_review_review_FK",
                table: "CandidateReviews");

            migrationBuilder.DropForeignKey(
                name: "candidate_review_stage_FK",
                table: "CandidateReviews");

            migrationBuilder.DropForeignKey(
                name: "candidate_to_stage_candidate_FK",
                table: "CandidateToStages");

            migrationBuilder.DropForeignKey(
                name: "candidate_to_stage_stage_FK",
                table: "CandidateToStages");

            migrationBuilder.DropForeignKey(
                name: "review_to_stage_review_FK",
                table: "ReviewToStages");

            migrationBuilder.DropForeignKey(
                name: "review_to_stage_stage_FK",
                table: "ReviewToStages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "ArchivedEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivedEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchivedEntities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchivedEntities_EntityType_EntityId",
                table: "ArchivedEntities",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_ArchivedEntities_UserId",
                table: "ArchivedEntities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "action_stage_FK",
                table: "Actions",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "apply_token__vacancy_FK",
                table: "ApplyTokens",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "candidate_comment_candidate_FK",
                table: "CandidateComments",
                column: "CandidateId",
                principalTable: "VacancyCandidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "candidate_comment_stage_FK",
                table: "CandidateComments",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "candidate_review_candidate_FK",
                table: "CandidateReviews",
                column: "CandidateId",
                principalTable: "VacancyCandidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "candidate_review_review_FK",
                table: "CandidateReviews",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "candidate_review_stage_FK",
                table: "CandidateReviews",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "candidate_to_stage_candidate_FK",
                table: "CandidateToStages",
                column: "CandidateId",
                principalTable: "VacancyCandidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "candidate_to_stage_stage_FK",
                table: "CandidateToStages",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "review_to_stage_review_FK",
                table: "ReviewToStages",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "review_to_stage_stage_FK",
                table: "ReviewToStages",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "action_stage_FK",
                table: "Actions");

            migrationBuilder.DropForeignKey(
                name: "apply_token__vacancy_FK",
                table: "ApplyTokens");

            migrationBuilder.DropForeignKey(
                name: "candidate_comment_candidate_FK",
                table: "CandidateComments");

            migrationBuilder.DropForeignKey(
                name: "candidate_comment_stage_FK",
                table: "CandidateComments");

            migrationBuilder.DropForeignKey(
                name: "candidate_review_candidate_FK",
                table: "CandidateReviews");

            migrationBuilder.DropForeignKey(
                name: "candidate_review_review_FK",
                table: "CandidateReviews");

            migrationBuilder.DropForeignKey(
                name: "candidate_review_stage_FK",
                table: "CandidateReviews");

            migrationBuilder.DropForeignKey(
                name: "candidate_to_stage_candidate_FK",
                table: "CandidateToStages");

            migrationBuilder.DropForeignKey(
                name: "candidate_to_stage_stage_FK",
                table: "CandidateToStages");

            migrationBuilder.DropForeignKey(
                name: "review_to_stage_review_FK",
                table: "ReviewToStages");

            migrationBuilder.DropForeignKey(
                name: "review_to_stage_stage_FK",
                table: "ReviewToStages");

            migrationBuilder.DropTable(
                name: "ArchivedEntities");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "action_stage_FK",
                table: "Actions",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "apply_token__vacancy_FK",
                table: "ApplyTokens",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "candidate_comment_candidate_FK",
                table: "CandidateComments",
                column: "CandidateId",
                principalTable: "VacancyCandidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "candidate_comment_stage_FK",
                table: "CandidateComments",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "candidate_review_candidate_FK",
                table: "CandidateReviews",
                column: "CandidateId",
                principalTable: "VacancyCandidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "candidate_review_review_FK",
                table: "CandidateReviews",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "candidate_review_stage_FK",
                table: "CandidateReviews",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "candidate_to_stage_candidate_FK",
                table: "CandidateToStages",
                column: "CandidateId",
                principalTable: "VacancyCandidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "candidate_to_stage_stage_FK",
                table: "CandidateToStages",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "review_to_stage_review_FK",
                table: "ReviewToStages",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "review_to_stage_stage_FK",
                table: "ReviewToStages",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
