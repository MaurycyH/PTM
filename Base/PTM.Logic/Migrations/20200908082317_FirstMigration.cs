using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PTM.Logic.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TaskBoard",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskBoard", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskBoard_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkItemCollection",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    TaskBoardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItemCollection", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkItemCollection_TaskBoard_TaskBoardId",
                        column: x => x.TaskBoardId,
                        principalTable: "TaskBoard",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    WorkItemStart = table.Column<DateTime>(nullable: false),
                    WorkItemEnd = table.Column<DateTime>(nullable: false),
                    WorkItemCollectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkItem_WorkItemCollection_WorkItemCollectionId",
                        column: x => x.WorkItemCollectionId,
                        principalTable: "WorkItemCollection",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskBoard_UserID",
                table: "TaskBoard",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItem_WorkItemCollectionId",
                table: "WorkItem",
                column: "WorkItemCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemCollection_TaskBoardId",
                table: "WorkItemCollection",
                column: "TaskBoardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkItem");

            migrationBuilder.DropTable(
                name: "WorkItemCollection");

            migrationBuilder.DropTable(
                name: "TaskBoard");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
