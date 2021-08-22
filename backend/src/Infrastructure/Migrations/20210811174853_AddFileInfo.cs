using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddFileInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvFileInfoId",
                table: "Applicants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInfos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CvFileInfoId",
                table: "Applicants",
                column: "CvFileInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_FileInfos_CvFileInfoId",
                table: "Applicants",
                column: "CvFileInfoId",
                principalTable: "FileInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_FileInfos_CvFileInfoId",
                table: "Applicants");

            migrationBuilder.DropTable(
                name: "FileInfos");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_CvFileInfoId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "CvFileInfoId",
                table: "Applicants");
        }
    }
}
