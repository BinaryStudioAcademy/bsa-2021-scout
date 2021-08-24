using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddSkillsParsingJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkillsParsingJobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TextPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutputPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TriggerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsParsingJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillsParsingJobs_Users_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillsParsingJobs_TriggerId",
                table: "SkillsParsingJobs",
                column: "TriggerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillsParsingJobs");
        }
    }
}
