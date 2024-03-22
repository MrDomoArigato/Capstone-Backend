using CapstoneApi.Data;
using CapstoneApi.Database;
using Microsoft.EntityFrameworkCore;


namespace CapstoneApi.Services;

public interface IAccountService
{
    Task<List<Account>?> GetAllAccounts();
    Account? GetAccount(int accountId);
    Account? CreateAccount(Account rawAccount);
    Account? UpdateAccount(Account updated);
    Account? UpdateBalance(int accountId, decimal amount);
    Account? DeleteAccount(int accountId);
}

public class AccountService(
    CapstoneContext context, 
    ILogger<AccountService> logger) : IAccountService{
    private readonly CapstoneContext _context = context;
    private readonly ILogger<AccountService> _logger = logger;

    public async Task<List<Account>?> GetAllAccounts(){
        return await _context.Accounts.ToListAsync();
    }

    public Account? GetAccount(int accountId){
        return _context.Accounts.Find(accountId);
    }

    public Account? CreateAccount(Account rawAccount){
        var cleanAccount = new Account{
            AccountId = 0,
            AccountName = rawAccount.AccountName,
            AccountOwner = rawAccount.AccountOwner,
            Balance = rawAccount.Balance,
            BalanceModification = DateTime.Now,
            ModificationDate = null,
            CreationDate = DateTime.Now
        };

        _context.Accounts.Add(cleanAccount);
        _context.SaveChanges();

        return GetAccount(cleanAccount.AccountId);
    }

    public Account? UpdateAccount(Account updated){
        var current = GetAccount(updated.AccountId);

        if(current is null)
            return null;
        
        current.AccountName = updated.AccountName;
        current.AccountOwner = updated.AccountOwner;
        current.ModificationDate = DateTime.Now;
        _context.SaveChanges();

        return GetAccount(updated.AccountId);
    }

    public Account? UpdateBalance(int accountId, decimal amount){
        var current = GetAccount(accountId);

        if(current is null)
            return null;

        current.Balance = amount;
        current.BalanceModification = DateTime.Now;
        _context.SaveChanges();

        return GetAccount(accountId);
    }

    public Account? DeleteAccount(int accountId){
        var delete = GetAccount(accountId);

        if(delete is null)
            return null;

        _context.Accounts.Remove(delete);
        _context.SaveChanges();

        //_tService.DeleteAccountTransactions(accountId);

        return delete;
    }
}