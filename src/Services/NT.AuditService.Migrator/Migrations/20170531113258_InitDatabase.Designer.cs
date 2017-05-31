using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NT.AuditService.Infrastructure;
using NT.AuditService.Core;

namespace NT.AuditService.Migrator.Migrations
{
    [DbContext(typeof(AuditDbContext))]
    [Migration("20170531113258_InitDatabase")]
    partial class InitDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NT.AuditService.Core.AuditInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActionMessage");

                    b.Property<int>("ActionType");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Source");

                    b.HasKey("Id");

                    b.ToTable("AuditInfos","audit");
                });
        }
    }
}
