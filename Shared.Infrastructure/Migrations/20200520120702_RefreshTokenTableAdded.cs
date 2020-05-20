using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shared.Infrastructure.Migrations
{
    public partial class RefreshTokenTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebTokens",
                columns: table => new
                {
                    TokenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(nullable: true),
                    AccessTokenExpiry = table.Column<DateTime>(nullable: false),
                    refreshToken = table.Column<string>(nullable: true),
                    refreshTokenExpiry = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebTokens", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_WebTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebTokens_UserId",
                table: "WebTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebTokens");
        }
    }
}
