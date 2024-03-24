﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShippingPricingStrategy.Infrastructure.Daos;

#nullable disable

namespace ShippingPricingStrategy.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240324125544_v1")]
    partial class v1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ShippingPricingStrategy.Domain.Models.Entities.Cart", b =>
                {
                    b.Property<long>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CartId"));

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("TotalAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal?>("TotalDiscount")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("CartId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("ShippingPricingStrategy.Domain.Models.Entities.Customer", b =>
                {
                    b.Property<long>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CustomerId"));

                    b.Property<string>("AddressLine")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NationalIdentifier")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("ShippingPricingStrategy.Domain.Models.Entities.Price", b =>
                {
                    b.Property<long>("PriceCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("PriceCode"));

                    b.Property<decimal>("IndividualPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal?>("MultiPurchasePrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int?>("QuantityPromotion")
                        .HasColumnType("int");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PriceCode");

                    b.ToTable("Price");
                });

            modelBuilder.Entity("ShippingPricingStrategy.Domain.Models.Entities.Service", b =>
                {
                    b.Property<long>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ServiceId"));

                    b.Property<long?>("CartId")
                        .HasColumnType("bigint");

                    b.Property<long>("PriceCode")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("ServiceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("int");

                    b.Property<int>("TotalDiscount")
                        .HasColumnType("int");

                    b.HasKey("ServiceId");

                    b.HasIndex("CartId");

                    b.HasIndex("PriceCode");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("ShippingPricingStrategy.Domain.Models.Entities.Cart", b =>
                {
                    b.HasOne("ShippingPricingStrategy.Domain.Models.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("ShippingPricingStrategy.Domain.Models.Entities.Service", b =>
                {
                    b.HasOne("ShippingPricingStrategy.Domain.Models.Entities.Cart", null)
                        .WithMany("Services")
                        .HasForeignKey("CartId");

                    b.HasOne("ShippingPricingStrategy.Domain.Models.Entities.Price", "Price")
                        .WithMany()
                        .HasForeignKey("PriceCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Price");
                });

            modelBuilder.Entity("ShippingPricingStrategy.Domain.Models.Entities.Cart", b =>
                {
                    b.Navigation("Services");
                });
#pragma warning restore 612, 618
        }
    }
}
