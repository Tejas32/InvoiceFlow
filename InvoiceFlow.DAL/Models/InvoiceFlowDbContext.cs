using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.DAL.Models;

public partial class InvoiceFlowDbContext : DbContext
{
    public InvoiceFlowDbContext()
    {
    }

    public InvoiceFlowDbContext(DbContextOptions<InvoiceFlowDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InvoiceFlowDB;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A24102A72F4");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithMany(p => p.Clients).HasConstraintName("FK__Clients__UserId__5165187F");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__D796AAB5A9EB34C0");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.Client).WithMany(p => p.Invoices).HasConstraintName("FK__Invoices__Client__5BE2A6F2");

            entity.HasOne(d => d.User).WithMany(p => p.Invoices).HasConstraintName("FK__Invoices__UserId__5AEE82B9");
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__InvoiceI__727E838B5021922D");

            entity.Property(e => e.Total).HasComputedColumnSql("([Quantity]*[Price])", false);

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceItems).HasConstraintName("FK__InvoiceIt__Invoi__5EBF139D");

            entity.HasOne(d => d.Product).WithMany(p => p.InvoiceItems).HasConstraintName("FK__InvoiceIt__Produ__5FB337D6");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD6C6E3CE2");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.User).WithMany(p => p.Products).HasConstraintName("FK__Products__UserId__5629CD9C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C328EA740");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
