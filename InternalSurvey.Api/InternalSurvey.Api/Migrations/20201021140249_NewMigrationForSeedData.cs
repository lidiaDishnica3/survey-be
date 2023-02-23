using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalSurvey.Api.Migrations
{
    public partial class NewMigrationForSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "e4dc78d1-91aa-4d3e-aa17-f244b5e72c63", new DateTime(2020, 10, 21, 16, 2, 49, 47, DateTimeKind.Local).AddTicks(9290), "ATISADMIN@ATIS.AL", "ATISADMIN@ATIS.AL", "AQAAAAEAACcQAAAAEPQsdgJx2Hj+/ZD7pLu/Tu5fJgfZVg/eP9IoLzFjv53wAimbkT18LXrgxncu7aT9BQ==", "639ef89c-2917-496c-9a94-e4d43d675c82", "atisadmin@atis.al" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a4c95d33-b702-499c-b436-621e786e7518",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { null, new DateTime(2020, 10, 8, 12, 58, 46, 709, DateTimeKind.Local).AddTicks(9892), null, null, "Admin123*", null, null });
        }
    }
}
