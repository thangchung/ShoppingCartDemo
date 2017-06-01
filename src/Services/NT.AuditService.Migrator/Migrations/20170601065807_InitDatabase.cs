using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NT.AuditService.Migrator.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "audit");

            migrationBuilder.CreateTable(
                name: "AuditInfos",
                schema: "audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActionMessage = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    MethodName = table.Column<string>(nullable: true),
                    ServiceName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditInfos",
                schema: "audit");
        }
    }
}
