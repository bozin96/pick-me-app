using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PickMeApp.Application.Migrations
{
    public partial class AddNotificationAndChatTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginCount",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "DriverId",
                table: "Rides",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "RouteIndex",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "AverageRate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRates",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstUserId = table.Column<string>(nullable: false),
                    SecondUserId = table.Column<string>(nullable: false),
                    LastMessageTimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_AspNetUsers_FirstUserId",
                        column: x => x.FirstUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chats_AspNetUsers_SecondUserId",
                        column: x => x.SecondUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    IsVisible = table.Column<bool>(nullable: false),
                    UserToId = table.Column<string>(nullable: true),
                    UserFromId = table.Column<string>(nullable: true),
                    RideId = table.Column<Guid>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Header = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserFromId",
                        column: x => x.UserFromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserToId",
                        column: x => x.UserToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PassengerOnRides",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RideId = table.Column<Guid>(nullable: false),
                    PassengerId = table.Column<string>(nullable: false),
                    Review = table.Column<int>(nullable: true),
                    StartWaypoint = table.Column<string>(nullable: true),
                    EndWaypoint = table.Column<string>(nullable: true),
                    DriverName = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerOnRides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassengerOnRides_AspNetUsers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerOnRides_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChatId = table.Column<Guid>(nullable: false),
                    SendUserId = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SendUserId",
                        column: x => x.SendUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DriverId",
                table: "Rides",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_FirstUserId",
                table: "Chats",
                column: "FirstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SecondUserId",
                table: "Chats",
                column: "SecondUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SendUserId",
                table: "Messages",
                column: "SendUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RideId",
                table: "Notifications",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserFromId",
                table: "Notifications",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserToId",
                table: "Notifications",
                column: "UserToId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerOnRides_PassengerId",
                table: "PassengerOnRides",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerOnRides_RideId",
                table: "PassengerOnRides",
                column: "RideId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_AspNetUsers_DriverId",
                table: "Rides",
                column: "DriverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_AspNetUsers_DriverId",
                table: "Rides");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PassengerOnRides");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Rides_DriverId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "RouteIndex",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "AverageRate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NumberOfRates",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "LoginCount",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);
        }
    }
}
