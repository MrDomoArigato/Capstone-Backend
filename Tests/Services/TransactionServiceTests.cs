using System.Diagnostics.CodeAnalysis;
using CapstoneApi.Data;
using CapstoneApi.Database;
using CapstoneApi.Services;
using Microsoft.EntityFrameworkCore;

namespace CapstoneApi.Tests.Services;

public class TransactionServiceTests : IDisposable 
{
    private readonly DbContextOptions<CapstoneContext> _options;
    private readonly CapstoneContext _context;

    public TransactionServiceTests()
    {
        _options = new DbContextOptionsBuilder<CapstoneContext>()
            .UseInMemoryDatabase(databaseName: "TestTransactionDatabase")
            .Options;

        _context = new CapstoneContext(_options);

        SeedData();
    }

    private void SeedData()
    {
        _context.Accounts.AddRange(new List<Account>{
            new() {
                AccountId = 1,
                AccountName = "TestAccount",
                AccountOwner = "test",
                Balance = 100.00m,
                BalanceModification = DateTime.Now,
                ModificationDate = DateTime.Now,
                CreationDate = DateTime.Now
            },
            new() {
                AccountId = 2,
                AccountName = "TestAccount2",
                AccountOwner = "test",
                Balance = 200.00m,
                BalanceModification = DateTime.Now,
                ModificationDate = DateTime.Now,
                CreationDate = DateTime.Now
            },
            new() {
                AccountId = 3,
                AccountName = "TestAccount3",
                AccountOwner = "test",
                Balance = 300.00m,
                BalanceModification = DateTime.Now,
                ModificationDate = DateTime.Now,
                CreationDate = DateTime.Now
            }
        });

        _context.Transactions.AddRange(new List<Transaction>{
            new() {
                TransactionId = 1,
                AccountId = 1,
                Amount = 100.00m,
                TransactionType = "001000",
                Description = "des1",
                TransactionDate = DateOnly.FromDateTime(DateTime.Now)
            },
            new() {
                TransactionId = 2,
                AccountId = 1,
                Amount = 100.00m,
                TransactionType = "001000",
                Description = "des2",
                TransactionDate = DateOnly.FromDateTime(DateTime.Now)
            },
            new() {
                TransactionId = 1,
                AccountId = 2,
                Amount = 100.00m,
                TransactionType = "001000",
                Description = "des3",
                TransactionDate = DateOnly.FromDateTime(DateTime.Now)
            },
            new() {
                TransactionId = 1,
                AccountId = 3,
                Amount = 100.00m,
                TransactionType = "001000",
                Description = "des1",
                TransactionDate = DateOnly.FromDateTime(DateTime.Now)
            },
            new() {
                TransactionId = 2,
                AccountId = 3,
                Amount = 100.00m,
                TransactionType = "001000",
                Description = "des2",
                TransactionDate = DateOnly.FromDateTime(DateTime.Now)
            },
        });

        _context.SaveChanges();
    }

    private void SeedTransactionTypes()
    {
        _context.TransactionTypes.AddRange(new List<TransactionType>{
            new(){
                Id = 001000,
                Code = "TESTC1",
                Description = "Test Code 1"
            },
            new() {
                Id = 001001,
                Code = "TESTC2",
                Description = "Test Code 2"
            },
            new() {
                Id = 002000,
                Code = "TESTC3",
                Description = "Test Code 3"
            },
            new() {
                Id = 002001,
                Code = "TESTC4",
                Description = "Test Code 4"
            },
            new() {
                Id = 003000,
                Code = "TESTC5",
                Description = "Test Code 5"
            },
            new() {
                Id = 003001,
                Code = "TESTC6",
                Description = "Test Code 6"
            }
        });

        _context.SaveChanges();
    }

    [Fact]
    public void GetTransactions_ReturnsCorrectTransactions()
    {
        var service = new TransactionService(_context);

        var returnedTransactions = service.GetTransactions(1);

        Assert.NotNull(returnedTransactions);
        Assert.NotNull(returnedTransactions.FirstOrDefault());
        Assert.Equal(2, returnedTransactions.Count);
        Assert.Equal(1, returnedTransactions.FirstOrDefault()!.AccountId);
    }

    [Fact]
    public async void GetTransactionsAsync_ReturnsCorrectTransactions()
    {
        var service = new TransactionService(_context);

        var returnedTransactions = await service.GetTransactionsAsync(1);

        Assert.NotNull(returnedTransactions);
        Assert.NotNull(returnedTransactions.FirstOrDefault());
        Assert.Equal(2, returnedTransactions.Count);
        Assert.Equal(1, returnedTransactions.FirstOrDefault()!.AccountId);
    }

    [Fact]
    public void GetTransaction_ReturnsCorrectTransaction()
    {
        var service = new TransactionService(_context);

        var returnedTransaction = service.GetTransaction(2, 1);

        Console.WriteLine(returnedTransaction);

        Assert.NotNull(returnedTransaction);
        Assert.Equal(2, returnedTransaction.AccountId);
        Assert.Equal(1, returnedTransaction.TransactionId);
    }

    [Fact]
    public async void GetTransactionAsync_ReturnsCorrectTransaction()
    {
        var service = new TransactionService(_context);

        var returnedTransaction = await service.GetTransactionAsync(2, 1);

        Assert.NotNull(returnedTransaction);
        Assert.Equal(2, returnedTransaction.AccountId);
        Assert.Equal(1, returnedTransaction.TransactionId);
    }

    [Fact]
    public void GetTransactionTypes_ReturnsCorrectTransactionTypes()
    {
        SeedTransactionTypes();
        var service = new TransactionService(_context);

        var returnedTypes = service.GetTransactionTypes();

        Assert.NotNull(returnedTypes);
        Assert.Equal(3, returnedTypes.Count);
        Assert.Equal(2, returnedTypes.FirstOrDefault()!.Count);
        Assert.Equal(001000, returnedTypes.FirstOrDefault()!.FirstOrDefault()!.Id);
        Assert.Equal("TESTC1", returnedTypes.FirstOrDefault()!.FirstOrDefault()!.Code);
    }

    [Fact]
    public void CreateTransaction_AddsTransactionToDatabase()
    {
        var service = new TransactionService(_context);

        var newTransaction = new Transaction {
            AccountId = 2,
            Amount = 100.00m,
            TransactionType = "001000",
            Description = "des1",
            TransactionDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var returnedTransaction = service.CreateTransaction(newTransaction);

        Assert.NotNull(returnedTransaction);
        Assert.Equal(newTransaction.AccountId, returnedTransaction.AccountId);
        Assert.Equal(2, returnedTransaction.TransactionId);
    }

    [Fact]
    public async void UpdateTransaction_UpdatesTransactionOnDatabase()
    {
        var service = new TransactionService(_context);

        var updatedTransaction = new Transaction {
            TransactionId = 1,
            AccountId = 2,
            Amount = 100.00m,
            TransactionType = "001001",
            Description = "updated",
            TransactionDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var returnedTransaction = await service.UpdateTransactionAsync(updatedTransaction);

        Assert.NotNull(returnedTransaction);
        Assert.Equal(updatedTransaction.AccountId, returnedTransaction.AccountId);
        Assert.Equal(updatedTransaction.Description, returnedTransaction.Description);
    }

    [Fact]
    public void DeleteTransaction_DeletesTransactionOnDatabase()
    {
        var startCount = _context.Transactions.Count();
        var service = new TransactionService(_context);

        var deletedTransaction = service.DeleteTransaction(2, 1);

        var endCount = _context.Accounts.Count();
        Assert.NotEqual(endCount, startCount);
    }

    [Fact]
    public void DeleteAccountTransactions_DeletesTransactionOnDatabase()
    {
        var startCount = _context.Transactions.Count();
        var service = new TransactionService(_context);

        var deletedTransactions = service.DeleteAccountTransactions(3);

        var endCount = _context.Transactions.Count();
        Assert.Equal(startCount - 2, endCount);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}