
using CapstoneApi.Data;
using CapstoneApi.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CapstoneApi.Services;

public interface ITransactionService
{
    List<Transaction> GetTransactions(int accountId);

    Task<List<Transaction>> GetTransactionsAsync(int accountId);

    Transaction? GetTransaction(int accountId, int transactionId);

    Task<Transaction?> GetTransactionAsync(int accountId, int transactionId);

    Transaction? CreateTransaction(Transaction rawTransaction);

    Task<Transaction?> UpdateTransactionAsync(Transaction updated);

    Transaction? DeleteTransaction(int accountId, int transactionId);

    List<Transaction> GetTransactionsOrderedByDate(int accountId, int TransactionDate);

    List<Transaction> GetTransactionsOrderedById(int accountId, int TransactionDate);
}

public class TransactionService(
    CapstoneContext context,
    ILogger<TransactionService> logger,
    IAccountService accountService) : ITransactionService {
    
    private readonly CapstoneContext _context = context;
    private readonly ILogger<TransactionService> _logger = logger;
    private readonly IAccountService _aService = accountService;

    private async Task<int> NewTransactionIdAsync(int accountId){
        if(!await _context.Transactions.AnyAsync())
            return 1;

        var transactions = _context.Transactions.Where(t => t.AccountId == accountId);
        if(!await transactions.AnyAsync())
            return 1;
            
        return await transactions.MaxAsync(t => t.TransactionId) + 1;
    }

    private int NewTransactionId(int accountId){
        if(!_context.Transactions.Any())
            return 1;

        var transactions = _context.Transactions.Where(t => t.AccountId == accountId);
        if(!transactions.Any())
            return 1;
            
        return transactions.Max(t => t.TransactionId) + 1;
    }

    public List<Transaction> GetTransactions(int accountId){
        return [.. _context.Transactions.Where(t => t.AccountId == accountId)];
    }

    public async Task<List<Transaction>> GetTransactionsAsync(int accountId){
        return await _context.Transactions.Where(t => t.AccountId == accountId).ToListAsync();
    }

    public Transaction? GetTransaction(int accountId, int transactionId){
        return _context.Transactions.Find(transactionId, accountId);
    }

    public async Task<Transaction?> GetTransactionAsync(int accountId, int transactionId){
        return await _context.Transactions.FindAsync(transactionId, accountId);
    }

    public Transaction? CreateTransaction(Transaction rawTransaction){
        var cleanTrans = new Transaction {
            TransactionId = NewTransactionId(rawTransaction.AccountId),
            AccountId = rawTransaction.AccountId,
            Amount = rawTransaction.Amount,
            TransactionType = rawTransaction.TransactionType,
            Description = rawTransaction.Description,
            TransactionDate = rawTransaction.TransactionDate,
            ModificationDate = null,
            CreationDate = DateTime.Now
        };

        _context.Transactions.Add(cleanTrans);
        _context.SaveChanges();

        _logger.LogDebug("test");

        return GetTransaction(cleanTrans.AccountId, cleanTrans.TransactionId);
    }

    public Transaction? UpdateTransaction(Transaction updated){
        var current = GetTransaction(updated.AccountId, updated.TransactionId);

        if(current is null)
            return null;

        current.Amount = updated.Amount;
        current.TransactionType = updated.TransactionType;
        current.Description = updated.Description;
        current.TransactionDate = updated.TransactionDate;
        current.ModificationDate = DateTime.Now;
        _context.SaveChanges();

        return GetTransaction(updated.AccountId, updated.TransactionId);
    }

    public async Task<Transaction?> UpdateTransactionAsync(Transaction updated){
        var current = await GetTransactionAsync(updated.AccountId, updated.TransactionId);

        if(current is null)
            return null;

        current.Amount = updated.Amount;
        current.TransactionType = updated.TransactionType;
        current.Description = updated.Description;
        current.TransactionDate = updated.TransactionDate;
        current.ModificationDate = DateTime.Now;
        await _context.SaveChangesAsync();

        return await GetTransactionAsync(updated.AccountId, updated.TransactionId);
    }

    public Transaction? DeleteTransaction(int accountId, int transactionId){
        var delete = GetTransaction(accountId, transactionId);

        if(delete is null)
            return null;
        
        _context.Transactions.Remove(delete);
        _context.SaveChanges();

        return delete;
    }

    
    public List<Transaction> GetTransactionsOrderedByDate(int accountId, int TransactionDate)
    {
        return _context.Transactions
            .OrderBy(t => t.TransactionDate.Year)
            .ThenBy(t => t.TransactionDate.Month)
            .ThenBy(t => t.TransactionDate.Day)
            .ToList();
    }

        public List<Transaction> GetTransactionsOrderedById(int accountId, int transactionId)
    {
        return _context.Transactions
            .OrderBy(t => t.TransactionId)
            .ToList();
    }
}
