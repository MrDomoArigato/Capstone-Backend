using CapstoneApi.Data;
using CapstoneApi.Database;
using CapstoneApi.Services;
using Microsoft.EntityFrameworkCore;

namespace CapstoneApi.Tests.Services;

public class AccountServiceTests : IDisposable 
{
    private readonly DbContextOptions<CapstoneContext> _options;
    private readonly CapstoneContext _context;

    public AccountServiceTests()
    {
        _options = new DbContextOptionsBuilder<CapstoneContext>()
            .UseInMemoryDatabase(databaseName: "TestAccountDatabase")
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
                AccountOwner = "test2",
                Balance = 300.00m,
                BalanceModification = DateTime.Now,
                ModificationDate = DateTime.Now,
                CreationDate = DateTime.Now
            }
        });

        _context.SaveChanges();
    }

    [Fact]
    public void GetUserAccounts_ReturnsCorrectAccounts()
    {
        var service = new AccountService(_context);

        var returnedAccounts = service.GetUserAccounts("test");

        Assert.NotNull(returnedAccounts);
        Assert.Equal(2, returnedAccounts.Count);
    }

    [Fact]
    public void GetUserAccounts_ReturnsEmptyList()
    {
        var service = new AccountService(_context);

        var returnedAccounts = service.GetUserAccounts("null");

        Assert.NotNull(returnedAccounts);
        Assert.Empty(returnedAccounts);
    }

    [Fact]
    public void GetAccounts_ReturnsCorrectAccount()
    {
        var expectedAccountId = 1;
        var service = new AccountService(_context);

        var returnedAccount = service.GetAccount(expectedAccountId);

        Assert.NotNull(returnedAccount);
        Assert.Equal(expectedAccountId, returnedAccount.AccountId);
    }

    [Fact]
    public void GetAccounts_ReturnsNull()
    {
        var expectedAccountId = 0;
        var service = new AccountService(_context);

        var returnedAccount = service.GetAccount(expectedAccountId);

        Assert.Null(returnedAccount);
    }

    [Fact]
    public void CreateAccount_AddsAccountToDatabase()
    {
        var startCount = _context.Accounts.Count();
        var service = new AccountService(_context);

        var createdAccount = new Account
        {
            AccountName = "NewAccount",
            AccountOwner = "AccountOwner",
            Balance = 100.00m   
        };

        var returnedAccount = service.CreateAccount(createdAccount);

        var endCount = _context.Accounts.Count();
        Assert.NotNull(returnedAccount);
        Assert.NotEqual(endCount, startCount);
        Assert.Equal(returnedAccount.AccountName, createdAccount.AccountName);
    }

    [Fact]
    public void UpdateAccount_UpdatesAccountOnDatabase()
    {
        var startCount = _context.Accounts.Count();
        var service = new AccountService(_context);

        var accountUpdate = new Account
        {
            AccountId = 1,
            AccountName = "NewAccount",
            AccountOwner = "AccountOwner",
            Balance = 100.00m,
            BalanceModification = DateTime.Now,
            ModificationDate = DateTime.Now,
            CreationDate = DateTime.Now
        };

        var updatedAccount = service.UpdateAccount(accountUpdate);

        var endCount = _context.Accounts.Count();
        Assert.NotNull(updatedAccount);
        Assert.Equal(endCount, startCount);
        Assert.Equal(updatedAccount.AccountId, accountUpdate.AccountId);
        Assert.Equal(updatedAccount.AccountName, accountUpdate.AccountName);
    }

    [Fact]
    public void DeleteAccount_DeletesAccountOnDatabase()
    {
        var deletedAccountId = 1;
        var startCount = _context.Accounts.Count();
        var service = new AccountService(_context);

        var deletedAccount = service.DeleteAccount(deletedAccountId);

        var endCount = _context.Accounts.Count();
        Assert.NotNull(deletedAccount);
        Assert.NotEqual(endCount, startCount);
        Assert.Equal(deletedAccount.AccountId, deletedAccountId);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}