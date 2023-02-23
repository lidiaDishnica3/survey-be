using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalSurvey.Api.Migrations
{
    public partial class addComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    SurveyId = table.Column<int>(nullable: false),
                    RespondentId = table.Column<int>(nullable: false),
                    CommnetText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Respondents_RespondentId",
                        column: x => x.RespondentId,
                        principalTable: "Respondents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "023168dc-35d5-45de-b757-62f544cb6a92", new DateTime(2020, 11, 17, 16, 29, 18, 674, DateTimeKind.Local).AddTicks(4638), "AQAAAAEAACcQAAAAEMpfhd+NM76ChIN4Kpb27ECxxURQ5HggRhfqEWy022NwP61xZT6KAvAYT08oSsks0w==", "23b1a109-bf36-4e7a-8f40-dd7762341d49" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RespondentId",
                table: "Comments",
                column: "RespondentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SurveyId",
                table: "Comments",
                column: "SurveyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e4dc78d1-91aa-4d3e-aa17-f244b5e72c63", new DateTime(2020, 10, 21, 16, 2, 49, 47, DateTimeKind.Local).AddTicks(9290), "AQAAAAEAACcQAAAAEPQsdgJx2Hj+/ZD7pLu/Tu5fJgfZVg/eP9IoLzFjv53wAimbkT18LXrgxncu7aT9BQ==", "639ef89c-2917-496c-9a94-e4d43d675c82" });
        }
    }
}
