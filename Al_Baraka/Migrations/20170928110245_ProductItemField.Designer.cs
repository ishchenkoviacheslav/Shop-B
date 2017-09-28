using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Al_Baraka.Models;

namespace Al_Baraka.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20170928110245_ProductItemField")]
    partial class ProductItemField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Al_Baraka.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Contact");

                    b.Property<bool>("IsDone");

                    b.Property<int>("Price");

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity");

                    b.Property<DateTime>("TimeOfBye");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Al_Baraka.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Country");

                    b.Property<string>("Description");

                    b.Property<byte[]>("Image");

                    b.Property<int>("Measure");

                    b.Property<string>("Name");

                    b.Property<int>("Price");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Al_Baraka.Models.ProductGroups", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("DriedFruits");

                    b.Property<bool>("EasternMed");

                    b.Property<bool>("Grocery");

                    b.Property<bool>("Italian");

                    b.Property<bool>("Nuts");

                    b.Property<bool>("Oils");

                    b.Property<int>("ProductId");

                    b.Property<bool>("Sauces");

                    b.Property<bool>("Spice");

                    b.Property<bool>("Sweets");

                    b.HasKey("Id");

                    b.HasIndex("ProductId")
                        .IsUnique();

                    b.ToTable("ProductGroups");
                });

            modelBuilder.Entity("Al_Baraka.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Contact");

                    b.Property<string>("Name");

                    b.Property<string>("ReviewText");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Al_Baraka.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Al_Baraka.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Login");

                    b.Property<string>("Password");

                    b.Property<int?>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Al_Baraka.Models.Order", b =>
                {
                    b.HasOne("Al_Baraka.Models.Product", "ProductItem")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Al_Baraka.Models.ProductGroups", b =>
                {
                    b.HasOne("Al_Baraka.Models.Product", "ProductForGroups")
                        .WithOne("Groups")
                        .HasForeignKey("Al_Baraka.Models.ProductGroups", "ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Al_Baraka.Models.User", b =>
                {
                    b.HasOne("Al_Baraka.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });
        }
    }
}
