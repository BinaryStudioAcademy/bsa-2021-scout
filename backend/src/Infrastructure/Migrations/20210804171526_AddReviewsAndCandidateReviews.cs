using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddReviewsAndCandidateReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CandidateReviews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CandidateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Mark = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateReviews", x => x.Id);
                    table.ForeignKey(
                        name: "candidate_review_candidate_FK",
                        column: x => x.CandidateId,
                        principalTable: "VacancyCandidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "candidate_review_review_FK",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "candidate_review_stage_FK",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateReviews_CandidateId",
                table: "CandidateReviews",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateReviews_ReviewId",
                table: "CandidateReviews",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateReviews_StageId",
                table: "CandidateReviews",
                column: "StageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateReviews");

            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
