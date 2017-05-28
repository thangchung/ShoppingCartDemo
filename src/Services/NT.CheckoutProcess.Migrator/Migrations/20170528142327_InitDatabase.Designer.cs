using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NT.CheckoutProcess.Infrastructure;

namespace NT.CheckoutProcess.Host.Migrations
{
    [DbContext(typeof(CheckoutProcessDbContext))]
    [Migration("20170528142327_InitDatabase")]
    partial class InitDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NT.CheckoutProcess.Core.SagaInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.HasKey("Id");

                    b.ToTable("SagaInfos","checkout");
                });
        }
    }
}
