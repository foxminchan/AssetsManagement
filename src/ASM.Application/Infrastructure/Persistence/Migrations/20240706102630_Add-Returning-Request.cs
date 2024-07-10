using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASM.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddReturningRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Staffs",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(6479),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(3992));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Staffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(6139),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(3651));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(8645),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(335));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(8255),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(52));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Assignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assignments",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(5801),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 19, DateTimeKind.Utc).AddTicks(8543));

            migrationBuilder.AlterColumn<Guid>(
                name: "StaffId",
                table: "Assignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(4980),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 19, DateTimeKind.Utc).AddTicks(8040));

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Assignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assets",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 627, DateTimeKind.Utc).AddTicks(9674),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 16, DateTimeKind.Utc).AddTicks(9217));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 627, DateTimeKind.Utc).AddTicks(9130),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 16, DateTimeKind.Utc).AddTicks(8815));

            migrationBuilder.CreateTable(
                name: "ReturningRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<byte>(type: "tinyint", nullable: false),
                    ReturnedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcceptBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(3581)),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(4177)),
                    Version = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturningRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturningRequests_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturningRequests_Staffs_AcceptBy",
                        column: x => x.AcceptBy,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturningRequests_AcceptBy",
                table: "ReturningRequests",
                column: "AcceptBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReturningRequests_AssignmentId",
                table: "ReturningRequests",
                column: "AssignmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturningRequests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Staffs",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(3992),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(6479));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Staffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(3651),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(6139));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(335),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(8645));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(52),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(8255));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Assignments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assignments",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 19, DateTimeKind.Utc).AddTicks(8543),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(5801));

            migrationBuilder.AlterColumn<Guid>(
                name: "StaffId",
                table: "Assignments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 19, DateTimeKind.Utc).AddTicks(8040),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(4980));

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Assignments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assets",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 16, DateTimeKind.Utc).AddTicks(9217),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 627, DateTimeKind.Utc).AddTicks(9674));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 16, DateTimeKind.Utc).AddTicks(8815),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 627, DateTimeKind.Utc).AddTicks(9130));
        }
    }
}
