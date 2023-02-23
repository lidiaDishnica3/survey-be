using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalSurvey.Api.Migrations
{
    public partial class AddSurveyResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SurveyResultId",
                table: "Responses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SurveyResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyResults_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                column: "CreatedOn",
                value: new DateTime(2020, 10, 4, 12, 6, 12, 62, DateTimeKind.Local).AddTicks(6532));

            migrationBuilder.CreateIndex(
                name: "IX_Responses_SurveyResultId",
                table: "Responses",
                column: "SurveyResultId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResults_SurveyId",
                table: "SurveyResults",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_SurveyResults_SurveyResultId",
                table: "Responses",
                column: "SurveyResultId",
                principalTable: "SurveyResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_SurveyResults_SurveyResultId",
                table: "Responses");

            migrationBuilder.DropTable(
                name: "SurveyResults");

            migrationBuilder.DropIndex(
                name: "IX_Responses_SurveyResultId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "SurveyResultId",
                table: "Responses");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                column: "CreatedOn",
                value: new DateTime(2020, 9, 30, 15, 49, 48, 658, DateTimeKind.Local).AddTicks(3417));
        }
    }
}
