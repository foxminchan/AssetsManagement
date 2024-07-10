using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASM.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Staffs",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(6956),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 18, 10, 47, 17, 663, DateTimeKind.Utc).AddTicks(4619));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Staffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(6678),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 18, 10, 47, 17, 663, DateTimeKind.Utc).AddTicks(4207));

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prefix = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(4307)),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(4609)),
                    Version = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Prefix",
                table: "Categories",
                column: "Prefix",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Staffs",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 6, 18, 10, 47, 17, 663, DateTimeKind.Utc).AddTicks(4619),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(6956));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Staffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 18, 10, 47, 17, 663, DateTimeKind.Utc).AddTicks(4207),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(6678));
        }
    }
}
