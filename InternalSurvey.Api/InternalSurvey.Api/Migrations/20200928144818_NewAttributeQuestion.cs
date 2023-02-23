using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalSurvey.Api.Migrations
{
    public partial class NewAttributeQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasOthers",
                table: "Questions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                column: "CreatedOn",
                value: new DateTime(2020, 9, 28, 16, 48, 17, 777, DateTimeKind.Local).AddTicks(383));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasOthers",
                table: "Questions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                column: "CreatedOn",
                value: new DateTime(2020, 9, 28, 10, 34, 1, 209, DateTimeKind.Local).AddTicks(749));
        }
    }
}
