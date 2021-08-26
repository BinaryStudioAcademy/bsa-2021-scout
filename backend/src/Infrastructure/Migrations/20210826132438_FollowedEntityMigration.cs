using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class FollowedEntityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFollowedEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowedEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFollowedEntities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowedEntities_UserId",
                table: "UserFollowedEntities",
                column: "UserId");
            migrationBuilder.CreateIndex(
                name: "IX_UserFollowedEntities_EntityId",
                table: "UserFollowedEntities",
                column: "EntityId");
            migrationBuilder.CreateIndex(
                name: "IX_UserFollowedEntities_EntityType",
                table: "UserFollowedEntities",
                column: "EntityType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFollowedEntities");
        }
    }
}
