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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "Name", "Prefix", "Version" },
                values: new object[,]
                {
                    { new Guid("039c5946-0dc0-4584-9494-8e00213cbff8"), new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(5476), "Personal Computer", "PC", new Guid("f3896495-6dc4-4125-b817-4d5cc538ee6e") },
                    { new Guid("266bd6bc-9231-44a9-b5c2-af567ac3df10"), new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(5492), "Laptop", "LT", new Guid("791c350c-7183-4ddb-bca5-797debbbe1ae") },
                    { new Guid("5047d5be-aeee-4072-ae47-b860ce5e0ae5"), new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(5502), "Printer", "PR", new Guid("bdf83dfb-6417-42cc-80c3-3bbd74ec7f56") },
                    { new Guid("6a5adb7b-94ee-498d-a20f-ec6e3d446df2"), new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(5511), "Monitor", "MN", new Guid("baab2a38-4cc1-4912-81f2-3e518b332ccb") },
                    { new Guid("7e9e0aa3-f1d1-46e8-8cf3-27fbeb85ed9c"), new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(5522), "Webcam", "WC", new Guid("29f46bd6-eaee-44b2-9f99-a65277684c00") },
                    { new Guid("a0c9e8a8-4321-4f74-9c9d-a6a881309dcd"), new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(5519), "Microphone", "MC", new Guid("340d5740-e4e9-4ed3-ace4-2336f98e017f") },
                    { new Guid("c0fadf90-721e-4b55-ac30-3567b63c8b8e"), new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(5505), "Bluetooth Mouse", "BM", new Guid("eb99681c-5406-48a9-92e6-6daffd0e52e1") },
                    { new Guid("c568c761-8916-4355-9991-247051cf7ea1"), new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(5515), "Keyboard", "KB", new Guid("d9c39d72-94bf-4446-819d-02f7e2cd44af") },
                    { new Guid("d1b6e7dd-e852-4c62-bee1-92c107d78bd6"), new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(5507), "Bluetooth Speaker", "BS", new Guid("e4b53e4f-9f8b-466e-890d-dca646d665dd") },
                    { new Guid("ef849fb7-10da-42aa-9c57-b07f8f33ee14"), new DateTime(2024, 6, 24, 8, 53, 32, 640, DateTimeKind.Utc).AddTicks(5517), "Headphone", "HP", new Guid("e68750e8-4b7b-406c-ac01-58ae4cef0c5b") }
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
