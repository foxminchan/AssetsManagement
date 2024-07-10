using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASM.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Removeseeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Staffs",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 509, DateTimeKind.Utc).AddTicks(2646),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 330, DateTimeKind.Utc).AddTicks(2320));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Staffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 509, DateTimeKind.Utc).AddTicks(2099),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 330, DateTimeKind.Utc).AddTicks(1330));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "ReturningRequests",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 508, DateTimeKind.Utc).AddTicks(8125),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 329, DateTimeKind.Utc).AddTicks(6867));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ReturningRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 508, DateTimeKind.Utc).AddTicks(4041),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 329, DateTimeKind.Utc).AddTicks(5631));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 507, DateTimeKind.Utc).AddTicks(7703),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(7879));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 507, DateTimeKind.Utc).AddTicks(6908),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(7512));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assignments",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 507, DateTimeKind.Utc).AddTicks(1483),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(4112));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 507, DateTimeKind.Utc).AddTicks(330),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(2627));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assets",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 505, DateTimeKind.Utc).AddTicks(9306),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 324, DateTimeKind.Utc).AddTicks(660));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 505, DateTimeKind.Utc).AddTicks(7867),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 324, DateTimeKind.Utc).AddTicks(122));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Staffs",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 330, DateTimeKind.Utc).AddTicks(2320),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 509, DateTimeKind.Utc).AddTicks(2646));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Staffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 330, DateTimeKind.Utc).AddTicks(1330),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 509, DateTimeKind.Utc).AddTicks(2099));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "ReturningRequests",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 329, DateTimeKind.Utc).AddTicks(6867),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 508, DateTimeKind.Utc).AddTicks(8125));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ReturningRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 329, DateTimeKind.Utc).AddTicks(5631),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 508, DateTimeKind.Utc).AddTicks(4041));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(7879),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 507, DateTimeKind.Utc).AddTicks(7703));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(7512),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 507, DateTimeKind.Utc).AddTicks(6908));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assignments",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(4112),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 507, DateTimeKind.Utc).AddTicks(1483));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(2627),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 507, DateTimeKind.Utc).AddTicks(330));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assets",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 324, DateTimeKind.Utc).AddTicks(660),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 505, DateTimeKind.Utc).AddTicks(9306));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 324, DateTimeKind.Utc).AddTicks(122),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 10, 4, 23, 58, 505, DateTimeKind.Utc).AddTicks(7867));
        }
    }
}
