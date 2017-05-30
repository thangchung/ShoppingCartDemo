using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NT.CheckoutProcess.Infrastructure;

namespace NT.CheckoutProcess.Migrator.Migrations
{
    [DbContext(typeof(CheckoutProcessDbContext))]
    partial class CheckoutProcessDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NT.CheckoutProcess.Core.SagaInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.Property<int>("SagaStatus");

                    b.HasKey("Id");

                    b.ToTable("SagaInfos","checkout");
                });
        }
    }
}
