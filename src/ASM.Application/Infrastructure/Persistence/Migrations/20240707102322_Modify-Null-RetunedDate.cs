﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASM.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifyNullRetunedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(6479));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Staffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 330, DateTimeKind.Utc).AddTicks(1330),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(6139));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "ReturningRequests",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 329, DateTimeKind.Utc).AddTicks(6867),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(4177));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReturnedDate",
                table: "ReturningRequests",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ReturningRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 329, DateTimeKind.Utc).AddTicks(5631),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(3581));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(7879),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(8645));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(7512),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(8255));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assignments",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(4112),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(5801));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(2627),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(4980));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assets",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 324, DateTimeKind.Utc).AddTicks(660),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 627, DateTimeKind.Utc).AddTicks(9674));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 324, DateTimeKind.Utc).AddTicks(122),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 627, DateTimeKind.Utc).AddTicks(9130));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 330, DateTimeKind.Utc).AddTicks(2320));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Staffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(6139),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 330, DateTimeKind.Utc).AddTicks(1330));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "ReturningRequests",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(4177),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 329, DateTimeKind.Utc).AddTicks(6867));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReturnedDate",
                table: "ReturningRequests",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ReturningRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 632, DateTimeKind.Utc).AddTicks(3581),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 329, DateTimeKind.Utc).AddTicks(5631));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(8645),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(7879));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(8255),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(7512));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assignments",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(5801),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(4112));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 631, DateTimeKind.Utc).AddTicks(4980),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 328, DateTimeKind.Utc).AddTicks(2627));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assets",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 627, DateTimeKind.Utc).AddTicks(9674),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 324, DateTimeKind.Utc).AddTicks(660));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 10, 26, 29, 627, DateTimeKind.Utc).AddTicks(9130),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 7, 10, 23, 21, 324, DateTimeKind.Utc).AddTicks(122));
        }
    }
}
