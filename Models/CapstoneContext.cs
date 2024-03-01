using CapstoneApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CapstoneApi.Database;

public partial class CapstoneContext : DbContext
{
    #warning TODO: change schema b4 release
    private readonly string _schema = "dev";

    public CapstoneContext()
    {
    }

    public CapstoneContext (DbContextOptions<CapstoneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<TransactionType> TransactionTypes { get; set; }
    public virtual DbSet<Budget> Budgets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => new { e.TransactionId, e.AccountId }).HasName("transactions_pkey");

            entity.ToTable("transactions", _schema);

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

            entity.ToTable("accounts", _schema);

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

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transactiontypes_pkey");

            entity.ToTable("transactiontypes", _schema);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<Budget>(entity => 
        {
            var jsonoptions = new JsonSerializerOptions(JsonSerializerDefaults.General);
            entity.HasKey(e => e.UserId).HasName("budgets_pkey");

            entity.ToTable("budgets", _schema);

            entity.Property(e => e.UserId).HasColumnName("userid");
            entity.Property(e => e.BudgetItems)
                .HasColumnName("budget")
                .HasColumnType("json")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonoptions),
                    v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, jsonoptions)
                );
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
