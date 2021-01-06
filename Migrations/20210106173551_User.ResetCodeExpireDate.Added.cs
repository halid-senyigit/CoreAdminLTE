using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreAdminLTE.Migrations
{
    public partial class UserResetCodeExpireDateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResetCodeExpireDate",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetCodeExpireDate",
                table: "Users");
        }
    }
}
