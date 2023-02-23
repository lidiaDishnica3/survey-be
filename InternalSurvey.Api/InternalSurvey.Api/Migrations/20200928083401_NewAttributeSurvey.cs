using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalSurvey.Api.Migrations
{
    public partial class NewAttributeSurvey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestionOptions_QuestionOrders_QuestionOrderId",
                table: "SurveyQuestionOptions");

            migrationBuilder.DropTable(
                name: "QuestionOrders");

            migrationBuilder.DropIndex(
                name: "IX_SurveyQuestionOptions_QuestionOrderId",
                table: "SurveyQuestionOptions");

            migrationBuilder.DropColumn(
                name: "QuestionOrderId",
                table: "SurveyQuestionOptions");

            migrationBuilder.AddColumn<string>(
                name: "SwitchOffRespondents",
                table: "Surveys",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VotingRespondents",
                table: "Surveys",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "SurveyQuestionOptions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SurveyId",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                column: "CreatedOn",
                value: new DateTime(2020, 9, 28, 10, 34, 1, 209, DateTimeKind.Local).AddTicks(749));

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionOptions_QuestionId",
                table: "SurveyQuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SurveyId",
                table: "Questions",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Surveys_SurveyId",
                table: "Questions",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestionOptions_Questions_QuestionId",
                table: "SurveyQuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Surveys_SurveyId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestionOptions_Questions_QuestionId",
                table: "SurveyQuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_SurveyQuestionOptions_QuestionId",
                table: "SurveyQuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_SurveyId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SwitchOffRespondents",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "VotingRespondents",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "SurveyQuestionOptions");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "QuestionOrderId",
                table: "SurveyQuestionOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestionOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOrders_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionOrders_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                column: "CreatedOn",
                value: new DateTime(2020, 9, 21, 15, 1, 1, 566, DateTimeKind.Local).AddTicks(1250));

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionOptions_QuestionOrderId",
                table: "SurveyQuestionOptions",
                column: "QuestionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOrders_QuestionId",
                table: "QuestionOrders",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOrders_SurveyId",
                table: "QuestionOrders",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestionOptions_QuestionOrders_QuestionOrderId",
                table: "SurveyQuestionOptions",
                column: "QuestionOrderId",
                principalTable: "QuestionOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
