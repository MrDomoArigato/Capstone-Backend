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

    #warning TODO: change schema b4 release
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
            entity.Property(e => e.CreationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creationdate");
            entity.Property(e => e.Description)
                .HasMaxLength(80)
                .HasColumnName("description");
            entity.Property(e => e.TransactionDate)
                .HasColumnName("transactiondate");
            entity.Property(e => e.ModificationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modificationdate");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(10)
                .HasColumnName("transactiontype");
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("accounts_pkey");

            entity.ToTable("accounts", "dev");

            entity.Property(e => e.AccountId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("accountid");
            entity.Property(e => e.AccountName)
                .HasMaxLength(25)
                .HasColumnName("accountname");
            entity.Property(e => e.AccountOwner)
                .HasMaxLength(20)
                .HasColumnName("accountowner");
            entity.Property(e => e.Balance)
                .HasPrecision(10, 2)
                .HasColumnName("balance");
            entity.Property(e => e.BalanceModification)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("balmodificationdate");
            entity.Property(e => e.ModificationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modificationdate");
            entity.Property(e => e.CreationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creationdate");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
