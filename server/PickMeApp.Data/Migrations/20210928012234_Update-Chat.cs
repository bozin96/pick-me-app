using Microsoft.EntityFrameworkCore.Migrations;

namespace PickMeApp.Application.Migrations
{
    public partial class UpdateChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastMessageSenderId",
                table: "Chats",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfUnreadedMessages",
                table: "Chats",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessageSenderId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "NumberOfUnreadedMessages",
                table: "Chats");
        }
    }
}
