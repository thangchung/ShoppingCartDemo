using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NT.CheckoutProcess.Migrator.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "checkout");

            migrationBuilder.CreateTable(
                name: "SagaInfos",
                schema: "checkout",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    SagaStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SagaInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SagaInfos",
                schema: "checkout");
        }
    }
}
