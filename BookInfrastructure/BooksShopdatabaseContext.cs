using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BookDomain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace BookInfrastructure;

public partial class BooksShopdatabaseContext : IdentityDbContext<IdentityUser> // Наслідуємося від IdentityDbContext
{
   
    
    public BooksShopdatabaseContext(DbContextOptions<BooksShopdatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<ShoppingBasket> ShoppingBaskets { get; set; }

    public virtual DbSet<ShoppingBasketBook> ShoppingBasketBooks { get; set; }

    public virtual DbSet<WarehouseBook> WarehouseBooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-35O34RUH\\SQLEXPRESS; Database=BooksShopdatabase; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__447D36EB06A970C8");

            entity.ToTable("Book");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Genre)
                .HasMaxLength(255)
                .HasColumnName("authorAddress");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(255)
                .HasColumnName("authorName");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("publisherName");
            entity.Property(e => e.Description)
         .HasColumnType("nvarchar(MAX)") 
         .HasColumnName("title");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.HasIndex(e => e.BookId, "IX_Category");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("categoryId");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("categoryName");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");

            
        });

       

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__4659622912A5D1BD");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("orderId");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(255)
                .HasColumnName("customerEmail");
            entity.Property(e => e.DeliveryAddress)
                .HasColumnType("text")
                .HasColumnName("deliveryAddress");
            entity.Property(e => e.OrderDate).HasColumnName("orderDate");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(255)
                .HasColumnName("orderStatus");
           
            entity.Property(e => e.ShoppingBasketId).HasColumnName("shoppingBasketId");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("totalPrice");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerEmail)
                .HasConstraintName("FK__Order__customer___44FF419A");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId); // Явно визначаємо первинний ключ
            entity.Property(e => e.OrderDetailId)
                .HasColumnName("orderDetailsId") // Явно вказуємо назву стовпця
                .ValueGeneratedNever(); // Якщо ID не генерується автоматично
            entity.Property(e => e.OrderId)
                .HasColumnName("orderId");
            entity.Property(e => e.BookId)
                .HasColumnName("bookId");
            entity.Property(e => e.Quantity)
                .HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnName("totalPrice")
                .HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Order");

            entity.HasOne(d => d.Book)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Book");
        });

        modelBuilder.Entity<ShoppingBasket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shopping__3214EC2790DBB2AE");

            entity.ToTable("ShoppingBasket");

            entity.Property(e => e.Id)
           .ValueGeneratedOnAdd()
              .HasColumnName("id");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(255)
                .HasColumnName("customerId");

            entity.HasOne(d => d.Customer).WithMany(p => p.ShoppingBaskets)
                .HasForeignKey(d => d.CustomerId)
               
               .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK__ShoppingB__Custo__46E78A0C");
        });

        modelBuilder.Entity<ShoppingBasketBook>(entity =>
        {
            entity.ToTable("ShoppingBasketBook");

            entity.HasKey(e => new { e.ShoppingBasketId, e.BookId }); // Складений ключ

            entity.Property(e => e.ShoppingBasketId).HasColumnName("shoppingBasketId");
            entity.Property(e => e.BookId).HasColumnName("bookId"); // Додано явне відображення
            entity.Property(e => e.Count).HasColumnName("count");

            entity.HasOne(d => d.Book)
                .WithMany(p => p.ShoppingBasketBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingBasketBook_Book");

            entity.HasOne(d => d.ShoppingBasket)
                .WithMany(p => p.ShoppingBasketBooks)
                .HasForeignKey(d => d.ShoppingBasketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingBasketBook_ShoppingBasket");
        });


        modelBuilder.Entity<WarehouseBook>(entity =>
        {
            entity.HasKey(e => new { e.WarehouseCode, e.BookId }).HasName("PK__Warehous__B56B0DC77A0EA844");

            entity.ToTable("WarehouseBook");

            entity.Property(e => e.WarehouseCode).HasColumnName("warehouseCode");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Count).HasColumnName("count");

            entity.HasOne(d => d.Book).WithMany(p => p.WarehouseBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__BookI__4AB81AF0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
