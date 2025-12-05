using System;
using System.Collections.Generic;
using HomeHubApi.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace HomeHubApi.Data;

public partial class HomeHubContext : DbContext
{
    public HomeHubContext()
    {
    }

    public HomeHubContext(DbContextOptions<HomeHubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CalendarEvent> CalendarEvents { get; set; }
    
    public virtual DbSet<Instruction> Instructions { get; set; }
    
    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.2.139;database=HomeHub;user=pi;password=Bhu8Nji9Mko0", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.8.3-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_uca1400_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CalendarEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("CalendarEvent");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Attendees).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TravelTime).HasColumnType("int(11)");
        });
        
        modelBuilder.Entity<Instruction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Instruction");

            entity.HasIndex(e => e.RecipeId, "Instruction_Recipe");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.RecipeId).HasColumnType("int(11)");
            entity.Property(e => e.SequenceNumber).HasColumnType("int(11)");
            entity.Property(e => e.Text).HasMaxLength(255);

            entity.HasOne(d => d.Recipe).WithMany(p => p.Instructions)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("Instruction_Recipe");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Barcode).HasMaxLength(255);
            entity.Property(e => e.Category).HasMaxLength(255);
            entity.Property(e => e.DefaultQuantity).HasPrecision(10);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Unit).HasColumnType("int(11)");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Recipe");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Duration).HasColumnType("int(11)");
            entity.Property(e => e.EffortRating)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)");
            entity.Property(e => e.HealthyRating)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Serves).HasColumnType("int(11)");
            entity.Property(e => e.TasteRating)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)");
            entity.Property(e => e.Type).HasColumnType("int(11)");
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("RecipeIngredient");

            entity.HasIndex(e => e.ProductId, "Product_Ingredient");

            entity.HasIndex(e => e.RecipeId, "Recipe_Ingredient");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.ProductId).HasColumnType("int(11)");
            entity.Property(e => e.QuantityRequired).HasPrecision(10);
            entity.Property(e => e.Unit).HasColumnType("int(11)");
            entity.Property(e => e.RecipeId).HasColumnType("int(11)");

            entity.HasOne(d => d.Product).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Product_Ingredient");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("Recipe_Ingredient");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Stock");

            entity.HasIndex(e => e.ProductId, "Product_Stock");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.ProductId).HasColumnType("int(11)");
            entity.Property(e => e.Quantity).HasPrecision(10);

            entity.HasOne(d => d.Product).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_Stock");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
