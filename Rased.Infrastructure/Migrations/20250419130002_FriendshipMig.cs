using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FriendshipMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_UserId1",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_UserId2",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_StaticFriendshipStatus_FriendshipStatusId",
                table: "Friendships");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "StaticFriendshipStatus");

            migrationBuilder.DropTable(
                name: "StaticFriendRequestStatus");

            migrationBuilder.DropIndex(
                name: "IX_Friendship_UserId1_UserId2",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_FriendshipStatusId",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "FriendshipStatusId",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "Nickname1",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "Nickname2",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "UserId2",
                table: "Friendships",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Friendships",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "FriendshipId",
                table: "Friendships",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_UserId2",
                table: "Friendships",
                newName: "IX_Friendships_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_UserId1",
                table: "Friendships",
                newName: "IX_Friendships_ReceiverId");

            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                table: "Friendships",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Friendships",
                type: "NVARCHAR(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Friendships",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_ReceiverId",
                table: "Friendships",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_SenderId",
                table: "Friendships",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_ReceiverId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_SenderId",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "SentAt",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Friendships",
                newName: "UserId2");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Friendships",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Friendships",
                newName: "FriendshipId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_SenderId",
                table: "Friendships",
                newName: "IX_Friendship_UserId2");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_ReceiverId",
                table: "Friendships",
                newName: "IX_Friendship_UserId1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Friendships",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FriendshipStatusId",
                table: "Friendships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nickname1",
                table: "Friendships",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nickname2",
                table: "Friendships",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StaticFriendRequestStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticFriendRequestStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticFriendshipStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticFriendshipStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FriendRequestStatusId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FriendRequests_StaticFriendRequestStatus_FriendRequestStatusId",
                        column: x => x.FriendRequestStatusId,
                        principalTable: "StaticFriendRequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_UserId1_UserId2",
                table: "Friendships",
                columns: new[] { "UserId1", "UserId2" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_FriendshipStatusId",
                table: "Friendships",
                column: "FriendshipStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_SenderId",
                table: "FriendRequests",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_FriendRequestStatusId",
                table: "FriendRequests",
                column: "FriendRequestStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_UserId1",
                table: "Friendships",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_UserId2",
                table: "Friendships",
                column: "UserId2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_StaticFriendshipStatus_FriendshipStatusId",
                table: "Friendships",
                column: "FriendshipStatusId",
                principalTable: "StaticFriendshipStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
