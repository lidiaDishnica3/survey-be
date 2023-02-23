using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalSurvey.Api.Migrations
{
    public partial class notRequiredFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Surveys",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Option",
                table: "SurveyQuestionOptions",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                column: "CreatedOn",
                value: new DateTime(2020, 10, 8, 12, 58, 46, 709, DateTimeKind.Local).AddTicks(9892));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Surveys",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Option",
                table: "SurveyQuestionOptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                column: "CreatedOn",
                value: new DateTime(2020, 10, 7, 13, 3, 37, 696, DateTimeKind.Local).AddTicks(2785));
        }
    }
}
