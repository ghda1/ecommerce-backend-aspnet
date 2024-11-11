﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20241111072127_imageNotuniq")]
    partial class imageNotuniq
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "material", new[] { "coton", "polyester", "blend", "linen", "crepe", "rayon", "silk" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "payment_method", new[] { "credit_card", "apple_pay", "cash" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "status", new[] { "delivered", "canceled", "shipped", "delayed" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Address", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("AddressName")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("character varying(55)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("character varying(55)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("character varying(55)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasMaxLength(55)
                        .HasColumnType("character varying(55)");

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Color", b =>
                {
                    b.Property<Guid>("ColorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ColorId");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("ColorProduct", b =>
                {
                    b.Property<Guid>("ColorsColorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductsProductId")
                        .HasColumnType("uuid");

                    b.HasKey("ColorsColorId", "ProductsProductId");

                    b.HasIndex("ProductsProductId");

                    b.ToTable("ColorProduct");
                });

            modelBuilder.Entity("Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("ShipmentId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OrderDetails", b =>
                {
                    b.Property<Guid>("OrdersDetailesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("ColorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("SizeId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.HasKey("OrdersDetailesId");

                    b.HasIndex("ColorId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SizeId");

                    b.ToTable("OrderDetailses");
                });

            modelBuilder.Entity("Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("PaymentId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Material")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ProductId");

                    b.HasIndex("Image");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ProductSize", b =>
                {
                    b.Property<Guid>("ProductsProductId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SizesSizeId")
                        .HasColumnType("uuid");

                    b.HasKey("ProductsProductId", "SizesSizeId");

                    b.HasIndex("SizesSizeId");

                    b.ToTable("ProductSize");
                });

            modelBuilder.Entity("Shipment", b =>
                {
                    b.Property<Guid>("ShipmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ShipmentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TrackingNumber")
                        .HasColumnType("integer");

                    b.HasKey("ShipmentId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("Size", b =>
                {
                    b.Property<Guid>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SizeId");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Address", b =>
                {
                    b.HasOne("User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ColorProduct", b =>
                {
                    b.HasOne("Color", null)
                        .WithMany()
                        .HasForeignKey("ColorsColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Order", b =>
                {
                    b.HasOne("User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OrderDetails", b =>
                {
                    b.HasOne("Color", "Color")
                        .WithMany()
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Product", "Product")
                        .WithMany("OrderDetailses")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Size", "Size")
                        .WithMany()
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Color");

                    b.Navigation("Order");

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("Payment", b =>
                {
                    b.HasOne("Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("Payment", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProductSize", b =>
                {
                    b.HasOne("Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Size", null)
                        .WithMany()
                        .HasForeignKey("SizesSizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shipment", b =>
                {
                    b.HasOne("Order", "Order")
                        .WithOne("Shipment")
                        .HasForeignKey("Shipment", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Order", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("Payment")
                        .IsRequired();

                    b.Navigation("Shipment")
                        .IsRequired();
                });

            modelBuilder.Entity("Product", b =>
                {
                    b.Navigation("OrderDetailses");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Orders");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
