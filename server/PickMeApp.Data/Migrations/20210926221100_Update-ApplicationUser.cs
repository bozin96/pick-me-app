using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PickMeApp.Application.Migrations
{
    public partial class UpdateApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "UserAvatarPhoto",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAvatarPhoto",
                table: "AspNetUsers");
        }
    }
}
