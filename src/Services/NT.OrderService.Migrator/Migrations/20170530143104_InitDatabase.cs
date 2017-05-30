using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NT.OrderService.Migrator.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shared");

            migrationBuilder.EnsureSchema(
                name: "order");

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
                name: "ShipInfos",
                schema: "shared",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AddressInfoId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipInfos_AddressInfos_AddressInfoId",
                        column: x => x.AddressInfoId,
                        principalSchema: "shared",
                        principalTable: "AddressInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderStatus = table.Column<int>(nullable: false),
                    SagaId = table.Column<Guid>(nullable: true),
                    ShipInfoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_ShipInfos_ShipInfoId",
                        column: x => x.ShipInfoId,
                        principalSchema: "shared",
                        principalTable: "ShipInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "order",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipInfoId",
                schema: "order",
                table: "Orders",
                column: "ShipInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                schema: "order",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipInfos_AddressInfoId",
                schema: "shared",
                table: "ShipInfos",
                column: "AddressInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails",
                schema: "order");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "order");

            migrationBuilder.DropTable(
                name: "ShipInfos",
                schema: "shared");

            migrationBuilder.DropTable(
                name: "AddressInfos",
                schema: "shared");
        }
    }
}
