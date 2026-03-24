using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryList.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    MappingsJson = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    InventoryTemplateId = table.Column<int>(type: "INTEGER", nullable: true),
                    ColumnSettingsJson = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryGroups_InventoryTemplates_InventoryTemplateId",
                        column: x => x.InventoryTemplateId,
                        principalTable: "InventoryTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InventoryGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    IP = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Domain = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Onboarded = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    RecoveryProcess = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    RAM = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Cores = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Version = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Edition = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    FullVersion = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CUVersion = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    KBVersion = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    LastPatchedOn = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Environment = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    OS = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Product = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Owner = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    StatusNotes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_InventoryGroups_InventoryGroupId",
                        column: x => x.InventoryGroupId,
                        principalTable: "InventoryGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryGroups_InventoryTemplateId",
                table: "InventoryGroups",
                column: "InventoryTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_InventoryGroupId",
                table: "InventoryItems",
                column: "InventoryGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "InventoryGroups");

            migrationBuilder.DropTable(
                name: "InventoryTemplates");
        }
    }
}
