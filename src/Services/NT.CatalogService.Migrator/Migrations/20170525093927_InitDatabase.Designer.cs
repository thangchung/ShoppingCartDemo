using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NT.CatalogService.Infrastructure;
using NT.CatalogService.Core;

namespace NT.CatalogService.Migrator.Migrations
{
    [DbContext(typeof(CatalogDbContext))]
    [Migration("20170525093927_InitDatabase")]
    partial class InitDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NT.CatalogService.Core.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Model");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<int>("Quantity");

                    b.Property<int>("Status");

                    b.Property<Guid>("SupplierId");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.ToTable("Products","catalog");
                });

            modelBuilder.Entity("NT.CatalogService.Core.Supplier", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AddressInfoId");

                    b.Property<string>("CompanyName");

                    b.Property<Guid?>("ContactInfoId");

                    b.Property<string>("ContactName");

                    b.Property<string>("ContactTitle");

                    b.HasKey("Id");

                    b.HasIndex("AddressInfoId");

                    b.HasIndex("ContactInfoId");

                    b.ToTable("Suppliers","catalog");
                });

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

            modelBuilder.Entity("NT.Core.SharedKernel.ContactInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Fax");

                    b.Property<string>("HomePage");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("ContactInfos","shared");
                });

            modelBuilder.Entity("NT.CatalogService.Core.Product", b =>
                {
                    b.HasOne("NT.CatalogService.Core.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NT.CatalogService.Core.Supplier", b =>
                {
                    b.HasOne("NT.Core.SharedKernel.AddressInfo", "AddressInfo")
                        .WithMany()
                        .HasForeignKey("AddressInfoId");

                    b.HasOne("NT.Core.SharedKernel.ContactInfo", "ContactInfo")
                        .WithMany()
                        .HasForeignKey("ContactInfoId");
                });
        }
    }
}
