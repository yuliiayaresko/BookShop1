﻿// <auto-generated />
using System;
using BookInfrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookInfrastructure.Migrations
{
    [DbContext(typeof(BooksShopdatabaseContext))]
    [Migration("20250309144817_UpdateOrderDetailsRelation")]
    partial class UpdateOrderDetailsRelation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookDomain.Model.Book", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("authorName");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(MAX)")
                        .HasColumnName("title");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("authorAddress");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("publisherName");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("price");

                    b.Property<int>("Year")
                        .HasColumnType("int")
                        .HasColumnName("year");

                    b.HasKey("Id")
                        .HasName("PK__Book__447D36EB06A970C8");

                    b.ToTable("Book", (string)null);
                });

            modelBuilder.Entity("BookDomain.Model.Category", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("categoryId");

                    b.Property<int?>("BookId")
                        .HasColumnType("int")
                        .HasColumnName("bookId");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("categoryName");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "BookId" }, "IX_Category");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("BookDomain.Model.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("orderId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("CustomerEmail")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("customerEmail");

                    b.Property<string>("DeliveryAddress")
                        .HasColumnType("text")
                        .HasColumnName("deliveryAddress");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("orderDate");

                    b.Property<string>("OrderStatus")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("orderStatus");

                    b.Property<int?>("ShoppingBasketId")
                        .HasColumnType("int")
                        .HasColumnName("shoppingBasketId");

                    b.Property<decimal?>("TotalPrice")
                        .HasColumnType("decimal(18, 0)")
                        .HasColumnName("totalPrice");

                    b.HasKey("OrderId")
                        .HasName("PK__Order__4659622912A5D1BD");

                    b.HasIndex("CustomerEmail");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("BookDomain.Model.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailsId")
                        .HasColumnType("int")
                        .HasColumnName("orderId");

                    b.Property<int>("BookId")
                        .HasColumnType("int")
                        .HasColumnName("bookId");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("status");

                    b.Property<decimal?>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("OrderDetailsId")
                        .HasName("PK_Order Details");

                    b.HasIndex("BookId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("BookDomain.Model.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .HasColumnType("int")
                        .HasColumnName("paymentId");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int")
                        .HasColumnName("orderId");

                    b.Property<DateOnly>("PaymentDate")
                        .HasColumnType("date")
                        .HasColumnName("paymentDate");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("paymentMethod");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("paymentStatus");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("transactionId");

                    b.HasKey("PaymentId")
                        .HasName("PK__Payment__ED1FC9EAA0D365BD");

                    b.HasIndex("OrderId");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("BookDomain.Model.ShoppingBasketBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.Property<int>("ShoppingBasketId")
                        .HasColumnType("int")
                        .HasColumnName("shoppingBasketID");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("ShoppingBasketId");

                    b.ToTable("ShoppingBasketBook", (string)null);
                });

            modelBuilder.Entity("BookDomain.Model.WarehouseBook", b =>
                {
                    b.Property<int>("WarehouseCode")
                        .HasColumnType("int")
                        .HasColumnName("warehouseCode");

                    b.Property<int>("BookId")
                        .HasColumnType("int")
                        .HasColumnName("bookId");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.HasKey("WarehouseCode", "BookId")
                        .HasName("PK__Warehous__B56B0DC77A0EA844");

                    b.HasIndex("BookId");

                    b.ToTable("WarehouseBook", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator().HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ShoppingBasket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("customerId");

                    b.HasKey("Id")
                        .HasName("PK__Shopping__3214EC2790DBB2AE");

                    b.HasIndex("CustomerId");

                    b.ToTable("ShoppingBasket", (string)null);
                });

            modelBuilder.Entity("BookDomain.Model.Customer", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("BookDomain.Model.Category", b =>
                {
                    b.HasOne("BookDomain.Model.Book", "Book")
                        .WithMany("Categories")
                        .HasForeignKey("BookId");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("BookDomain.Model.Order", b =>
                {
                    b.HasOne("BookDomain.Model.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerEmail")
                        .HasConstraintName("FK__Order__customer___44FF419A");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("BookDomain.Model.OrderDetail", b =>
                {
                    b.HasOne("BookDomain.Model.Book", "Book")
                        .WithMany("OrderDetails")
                        .HasForeignKey("BookId")
                        .IsRequired()
                        .HasConstraintName("FK_Order Details_Book");

                    b.HasOne("BookDomain.Model.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK_Order Details_Order");

                    b.Navigation("Book");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("BookDomain.Model.Payment", b =>
                {
                    b.HasOne("BookDomain.Model.Order", "Order")
                        .WithMany("Payments")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK__Payment__order_i__45F365D3");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("BookDomain.Model.ShoppingBasketBook", b =>
                {
                    b.HasOne("BookDomain.Model.Book", "Book")
                        .WithMany("ShoppingBasketBooks")
                        .HasForeignKey("BookId")
                        .IsRequired()
                        .HasConstraintName("FK_ShoppingBasketBook_Book");

                    b.HasOne("ShoppingBasket", "ShoppingBasket")
                        .WithMany("ShoppingBasketBooks")
                        .HasForeignKey("ShoppingBasketId")
                        .IsRequired()
                        .HasConstraintName("FK__ShoppingB__Shopp__47DBAE45");

                    b.Navigation("Book");

                    b.Navigation("ShoppingBasket");
                });

            modelBuilder.Entity("BookDomain.Model.WarehouseBook", b =>
                {
                    b.HasOne("BookDomain.Model.Book", "Book")
                        .WithMany("WarehouseBooks")
                        .HasForeignKey("BookId")
                        .IsRequired()
                        .HasConstraintName("FK__Warehouse__BookI__4AB81AF0");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShoppingBasket", b =>
                {
                    b.HasOne("BookDomain.Model.Customer", "Customer")
                        .WithMany("ShoppingBaskets")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK__ShoppingB__Custo__46E78A0C");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("BookDomain.Model.Book", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("OrderDetails");

                    b.Navigation("ShoppingBasketBooks");

                    b.Navigation("WarehouseBooks");
                });

            modelBuilder.Entity("BookDomain.Model.Order", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("ShoppingBasket", b =>
                {
                    b.Navigation("ShoppingBasketBooks");
                });

            modelBuilder.Entity("BookDomain.Model.Customer", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("ShoppingBaskets");
                });
#pragma warning restore 612, 618
        }
    }
}
