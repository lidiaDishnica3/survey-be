using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalSurvey.Api.Migrations
{
    public partial class addComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e6deaa16-34ee-45fb-b219-c608da67caa6", new DateTime(2020, 11, 17, 16, 30, 42, 167, DateTimeKind.Local).AddTicks(1059), "AQAAAAEAACcQAAAAENl767SDv10vi7qn8FQl66v1qYw/LBX6BM1aEsAN4UmbI7nEQPecqg9MuK+7RgMfRA==", "30c126a7-876b-4998-bdee-84d92d4925fd" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "023168dc-35d5-45de-b757-62f544cb6a92", new DateTime(2020, 11, 17, 16, 29, 18, 674, DateTimeKind.Local).AddTicks(4638), "AQAAAAEAACcQAAAAEMpfhd+NM76ChIN4Kpb27ECxxURQ5HggRhfqEWy022NwP61xZT6KAvAYT08oSsks0w==", "23b1a109-bf36-4e7a-8f40-dd7762341d49" });
        }
    }
}
