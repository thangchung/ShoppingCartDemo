using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NT.CustomerService.Infrastructure;

namespace NT.CustomerService.Migrator.Migrations
{
    [DbContext(typeof(CustomerDbContext))]
    [Migration("20170523101016_InitDatabase")]
    partial class InitDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("NT.CustomerService.Core.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AddressInfoId");

                    b.Property<Guid>("ContactInfoId");

                    b.Property<string>("ContactTitle");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.HasIndex("AddressInfoId");

                    b.HasIndex("ContactInfoId");

                    b.ToTable("Customers","customer");
                });

            modelBuilder.Entity("NT.CustomerService.Core.Customer", b =>
                {
                    b.HasOne("NT.Core.SharedKernel.AddressInfo", "AddressInfo")
                        .WithMany()
                        .HasForeignKey("AddressInfoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NT.Core.SharedKernel.ContactInfo", "ContactInfo")
                        .WithMany()
                        .HasForeignKey("ContactInfoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
