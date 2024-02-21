using System;
using System.Collections.Generic;
using CapstoneApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CapstoneApi.Database;

public partial class CapstoneContext : DbContext
{
    public CapstoneContext()
    {
    }

    public CapstoneContext(DbContextOptions<CapstoneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => new { e.TransactionId, e.AccountId }).HasName("transactions_pkey");

            entity.ToTable("transactions", "dev");

            entity.Property(e => e.TransactionId).HasColumnName("transactionid");
            entity.Property(e => e.AccountId).HasColumnName("accountid");
            entity.Property(e => e.Amount)
                .HasPrecision(8, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Balance)
                .HasPrecision(10, 2)
                .HasColumnName("balance");
            entity.Property(e => e.CreationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creationdate");
            entity.Property(e => e.Description)
                .HasMaxLength(80)
                .HasColumnName("description");
            entity.Property(e => e.ModificationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modificationdate");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(10)
                .HasColumnName("transactiontype");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
