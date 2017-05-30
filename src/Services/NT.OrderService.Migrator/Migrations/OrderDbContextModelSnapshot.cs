using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NT.OrderService.Infrastructure;
using NT.OrderService.Core;

namespace NT.OrderService.Migrator.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    partial class OrderDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NT.Core.SharedKernel.AddressInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("PostalCode");

                    b.Property<string>("Region");

                    b.HasKey("Id");

                    b.ToTable("AddressInfos","shared");
                });

            modelBuilder.Entity("NT.OrderService.Core.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CustomerId");

                    b.Property<Guid>("EmployeeId");

                    b.Property<DateTime>("OrderDate");

                    b.Property<int>("OrderStatus");

                    b.Property<Guid?>("SagaId");

                    b.Property<Guid?>("ShipInfoId");

                    b.HasKey("Id");

                    b.HasIndex("ShipInfoId");

                    b.ToTable("Orders","order");
                });

            modelBuilder.Entity("NT.OrderService.Core.OrderDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("OrderId");

                    b.Property<Guid>("ProductId");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails","order");
                });

            modelBuilder.Entity("NT.OrderService.Core.ShipInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AddressInfoId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AddressInfoId");

                    b.ToTable("ShipInfos","shared");
                });

            modelBuilder.Entity("NT.OrderService.Core.Order", b =>
                {
                    b.HasOne("NT.OrderService.Core.ShipInfo", "ShipInfo")
                        .WithMany()
                        .HasForeignKey("ShipInfoId");
                });

            modelBuilder.Entity("NT.OrderService.Core.OrderDetail", b =>
                {
                    b.HasOne("NT.OrderService.Core.Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("NT.OrderService.Core.ShipInfo", b =>
                {
                    b.HasOne("NT.Core.SharedKernel.AddressInfo", "AddressInfo")
                        .WithMany()
                        .HasForeignKey("AddressInfoId");
                });
        }
    }
}
