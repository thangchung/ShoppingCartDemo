using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NT.PaymentService.Infrastructure;
using NT.PaymentService.Core;

namespace NT.PaymentService.Migrator.Migrations
{
    [DbContext(typeof(PaymentDbContext))]
    [Migration("20170531073039_InitDatabase")]
    partial class InitDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NT.PaymentService.Core.CustomerPayment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CustomerId");

                    b.Property<Guid>("EmployeeId");

                    b.Property<double>("Money");

                    b.Property<Guid>("OrderId");

                    b.Property<Guid>("PaymentMethodId");

                    b.Property<int>("PaymentStatus");

                    b.HasKey("Id");

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("CustomerPayments","payment");
                });

            modelBuilder.Entity("NT.PaymentService.Core.PaymentMethod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.HasKey("Id");

                    b.ToTable("PaymentMethods","payment");
                });

            modelBuilder.Entity("NT.PaymentService.Core.CustomerPayment", b =>
                {
                    b.HasOne("NT.PaymentService.Core.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
