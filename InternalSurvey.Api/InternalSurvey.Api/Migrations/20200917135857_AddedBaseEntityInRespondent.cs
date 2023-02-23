using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalSurvey.Api.Migrations
{
    public partial class AddedBaseEntityInRespondent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Respondents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Respondents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Respondents",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Respondents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Respondents",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Respondents",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2020, 9, 17, 15, 58, 57, 25, DateTimeKind.Local).AddTicks(2736));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Respondents");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Respondents");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Respondents");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Respondents");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Respondents");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Respondents");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2020, 9, 17, 11, 50, 44, 823, DateTimeKind.Local).AddTicks(684));
        }
    }
}
