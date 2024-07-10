using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASM.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Staffs",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(3992),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 271, DateTimeKind.Utc).AddTicks(6870));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Staffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(3651),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 271, DateTimeKind.Utc).AddTicks(6417));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(335),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 271, DateTimeKind.Utc).AddTicks(2597));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(52),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 271, DateTimeKind.Utc).AddTicks(2158));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assets",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 16, DateTimeKind.Utc).AddTicks(9217),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 268, DateTimeKind.Utc).AddTicks(2744));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 16, DateTimeKind.Utc).AddTicks(8815),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 268, DateTimeKind.Utc).AddTicks(2418));

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<byte>(type: "tinyint", nullable: false),
                    AssignedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 19, DateTimeKind.Utc).AddTicks(8040)),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 19, DateTimeKind.Utc).AddTicks(8543)),
                    Version = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Assignments_Staffs_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssetId",
                table: "Assignments",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_UpdatedBy",
                table: "Assignments",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Staffs",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 271, DateTimeKind.Utc).AddTicks(6870),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(3992));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Staffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 271, DateTimeKind.Utc).AddTicks(6417),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(3651));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 271, DateTimeKind.Utc).AddTicks(2597),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(335));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 271, DateTimeKind.Utc).AddTicks(2158),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 20, DateTimeKind.Utc).AddTicks(52));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Assets",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 268, DateTimeKind.Utc).AddTicks(2744),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 16, DateTimeKind.Utc).AddTicks(9217));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 14, 19, 26, 268, DateTimeKind.Utc).AddTicks(2418),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 14, 22, 30, 16, DateTimeKind.Utc).AddTicks(8815));
        }
    }
}
