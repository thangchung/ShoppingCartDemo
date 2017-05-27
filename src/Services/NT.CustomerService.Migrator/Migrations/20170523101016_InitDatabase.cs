using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NT.CustomerService.Migrator.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shared");

            migrationBuilder.EnsureSchema(
                name: "customer");

            migrationBuilder.CreateTable(
                name: "AddressInfos",
                schema: "shared",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfos",
                schema: "shared",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Fax = table.Column<string>(nullable: true),
                    HomePage = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AddressInfoId = table.Column<Guid>(nullable: false),
                    ContactInfoId = table.Column<Guid>(nullable: false),
                    ContactTitle = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AddressInfos_AddressInfoId",
                        column: x => x.AddressInfoId,
                        principalSchema: "shared",
                        principalTable: "AddressInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_ContactInfos_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalSchema: "shared",
                        principalTable: "ContactInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AddressInfoId",
                schema: "customer",
                table: "Customers",
                column: "AddressInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ContactInfoId",
                schema: "customer",
                table: "Customers",
                column: "ContactInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers",
                schema: "customer");

            migrationBuilder.DropTable(
                name: "AddressInfos",
                schema: "shared");

            migrationBuilder.DropTable(
                name: "ContactInfos",
                schema: "shared");
        }
    }
}
