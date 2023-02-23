using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalSurvey.Api.Migrations
{
    public partial class addEntityModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Questions_QuestionId",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_SurveyResponses_SurveyResponseId",
                table: "Responses");

            migrationBuilder.DropTable(
                name: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_QuestionId",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_SurveyResponseId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "SurveyResponseId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "Options",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "Other",
                table: "Responses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RespondentId",
                table: "Responses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SurveyQuestionOptionsId",
                table: "Responses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SurveyQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Option = table.Column<string>(nullable: true),
                    QuestionOrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyQuestionOptions_QuestionOrders_QuestionOrderId",
                        column: x => x.QuestionOrderId,
                        principalTable: "QuestionOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2020, 9, 17, 11, 47, 56, 958, DateTimeKind.Local).AddTicks(8654));

            migrationBuilder.CreateIndex(
                name: "IX_Responses_RespondentId",
                table: "Responses",
                column: "RespondentId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_SurveyQuestionOptionsId",
                table: "Responses",
                column: "SurveyQuestionOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionOptions_QuestionOrderId",
                table: "SurveyQuestionOptions",
                column: "QuestionOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Respondents_RespondentId",
                table: "Responses",
                column: "RespondentId",
                principalTable: "Respondents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_SurveyQuestionOptions_SurveyQuestionOptionsId",
                table: "Responses",
                column: "SurveyQuestionOptionsId",
                principalTable: "SurveyQuestionOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Respondents_RespondentId",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_SurveyQuestionOptions_SurveyQuestionOptionsId",
                table: "Responses");

            migrationBuilder.DropTable(
                name: "SurveyQuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_Responses_RespondentId",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_SurveyQuestionOptionsId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "RespondentId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "SurveyQuestionOptionsId",
                table: "Responses");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "Responses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Responses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SurveyResponseId",
                table: "Responses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SurveyResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HasVoted = table.Column<bool>(type: "bit", nullable: false),
                    RespondentId = table.Column<int>(type: "int", nullable: false),
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyResponses_Respondents_RespondentId",
                        column: x => x.RespondentId,
                        principalTable: "Respondents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyResponses_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2020, 9, 16, 15, 58, 23, 34, DateTimeKind.Local).AddTicks(3274));

            migrationBuilder.CreateIndex(
                name: "IX_Responses_QuestionId",
                table: "Responses",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_SurveyResponseId",
                table: "Responses",
                column: "SurveyResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_RespondentId",
                table: "SurveyResponses",
                column: "RespondentId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SurveyId",
                table: "SurveyResponses",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Questions_QuestionId",
                table: "Responses",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_SurveyResponses_SurveyResponseId",
                table: "Responses",
                column: "SurveyResponseId",
                principalTable: "SurveyResponses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
