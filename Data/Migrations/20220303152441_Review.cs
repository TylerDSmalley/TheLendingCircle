using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheLendingCircle.Data.Migrations
{
    public partial class Review : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HasBeenViewed = table.Column<bool>(type: "INTEGER", nullable: false),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: false),
                    BorrowerId = table.Column<string>(type: "TEXT", nullable: false),
                    ItemLoanedId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Request_AspNetUsers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Request_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Request_Items_ItemLoanedId",
                        column: x => x.ItemLoanedId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReviewBody = table.Column<string>(type: "TEXT", maxLength: 5000, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Rating = table.Column<double>(type: "REAL", nullable: false),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: false),
                    BorrowerId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_AspNetUsers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Request_BorrowerId",
                table: "Request",
                column: "BorrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_ItemLoanedId",
                table: "Request",
                column: "ItemLoanedId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_OwnerId",
                table: "Request",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_BorrowerId",
                table: "Review",
                column: "BorrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_OwnerId",
                table: "Review",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "Review");
        }
    }
}
