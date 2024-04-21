using CapstoneApi.Data;
using CapstoneApi.Database;
using Microsoft.EntityFrameworkCore;

namespace CapstoneApi.Services;

public interface ITransactionService
{
    List<Transaction> GetTransactions(int accountId);

    Task<List<Transaction>> GetTransactionsAsync(int accountId);

    List<List<TransactionType>> GetTransactionTypes();

    Transaction? GetTransaction(int accountId, int transactionId);

    Task<Transaction?> GetTransactionAsync(int accountId, int transactionId);

    Transaction? CreateTransaction(Transaction rawTransaction);

    Task<Transaction?> UpdateTransactionAsync(Transaction updated);

    Transaction? DeleteTransaction(int accountId, int transactionId);

    List<Transaction> DeleteAccountTransactions(int accountId);
}

public class TransactionService(
    CapstoneContext context
    ) : ITransactionService {
    
    private readonly CapstoneContext _context = context;

    /// <summary>Asyncronous Method calulates next available <c>TransactionId</c> from database.
    /// (<paramref name="accountId"/>)
    /// </summary>
    /// <param name="accountId"><c>AccountId</c> used for finding new sequence id in database</param>
    /// <returns>Next available TransactionId</returns>
    private async Task<int> NewTransactionIdAsync(int accountId){
        if(!await _context.Transactions.AnyAsync())
            return 1;

        var transactions = _context.Transactions.Where(t => t.AccountId == accountId);
        if(!await transactions.AnyAsync())
            return 1;
            
        return await transactions.MaxAsync(t => t.TransactionId) + 1;
    }

    /// <summary>Method calulates next available <c>TransactionId</c> from database.
    /// (<paramref name="accountId"/>)
    /// </summary>
    /// <param name="accountId"><c>AccountId</c> used for finding new sequence id in database</param>
    /// <returns>Next available TransactionId</returns>
    private int NewTransactionId(int accountId){
        if(!_context.Transactions.Any())
            return 1;

        var transactions = _context.Transactions.Where(t => t.AccountId == accountId);
        if(!transactions.Any())
            return 1;
            
        return transactions.Max(t => t.TransactionId) + 1;
    }

    /// <summary>Method finds all <see cref="Transaction"/>s that are associated with an <c>accountId</c>
    /// (<paramref name="accountId"/>)
    /// </summary>
    /// <param name="accountId"><c>AccountId</c> used for finding all transactions under account</param>
    /// <returns><see cref="List<>"/> of <see cref="Transaction"/>s on an Account</returns>
    public List<Transaction> GetTransactions(int accountId){
        return [.. _context.Transactions.Where(t => t.AccountId == accountId).OrderByDescending(t => t.TransactionDate)];
    }

    /// <summary>Asyncronous Method finds all <see cref="Transaction"/>s that are associated with an <c>accountId</c>
    /// (<paramref name="accountId"/>)
    /// </summary>
    /// <param name="accountId"><c>AccountId</c> used for finding all transactions under account</param>
    /// <returns><see cref="List<>"/> of <see cref="Transaction"/>s on an Account</returns>
    public async Task<List<Transaction>> GetTransactionsAsync(int accountId){
        return await _context.Transactions.Where(t => t.AccountId == accountId).OrderByDescending(t => t.TransactionDate).ToListAsync();
    }

    public List<List<TransactionType>> GetTransactionTypes(){
        var rawTypes = _context.TransactionTypes.OrderBy(t => t.Id).ToList();
        var types = new List<List<TransactionType>>{};
        var subs = new List<TransactionType>{};
        foreach (TransactionType type in rawTypes)
        {
            if(type.Id % 1000 == 0){
                if(subs.Count > 0)
                    types.Add(subs);
                subs = [];
            } 
            subs.Add(type);
        }
        types.Add(subs);
        return types;
    }

    /// <summary>Method finds a single <see cref="Transaction"/> using Primary Key
    /// (<paramref name="accountId"/>, <paramref name="transactionId"/>)
    /// </summary>
    /// <param name="accountId"><c>AccountId</c> used in Primary Key</param>
    /// <param name="transactionId"><c>TransactionId</c> used in Primary Key</param>
    /// <returns>A <see cref="Transaction"/> matching supplied Primary Key</returns>
    public Transaction? GetTransaction(int accountId, int transactionId){
        return _context.Transactions.Find(transactionId, accountId);
    }

    /// <summary>Asyncronous Method finds a single <see cref="Transaction"/> using Primary Key
    /// (<paramref name="accountId"/>, <paramref name="transactionId"/>)
    /// </summary>
    /// <param name="accountId"><c>AccountId</c> used in Primary Key</param>
    /// <param name="transactionId"><c>TransactionId</c> used in Primary Key</param>
    /// <returns>A <see cref="Transaction"/> matching supplied Primary Key</returns>
    public async Task<Transaction?> GetTransactionAsync(int accountId, int transactionId){
        return await _context.Transactions.FindAsync(transactionId, accountId);
    }

    /// <summary>Method creates a single <see cref="Transaction"/>
    /// (<paramref name="rawTransaction"/>)
    /// </summary>
    /// <param name="rawTransaction"><see cref="Transaction"/> holds necessary information for saving</param>
    /// <returns>Created <see cref="Transaction"/></returns>
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

        return GetTransaction(cleanTrans.AccountId, cleanTrans.TransactionId);
    }

    /// <summary>Method updates a single <see cref="Transaction"/>
    /// (<paramref name="updated"/>)
    /// </summary>
    /// <param name="updated"><see cref="Transaction"/> holds necessary information for looking up & updating</param>
    /// <returns>Updated <see cref="Transaction"/></returns>
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

    /// <summary>Asyncronous Method updates a single <see cref="Transaction"/>
    /// (<paramref name="updated"/>)
    /// </summary>
    /// <param name="updated"><see cref="Transaction"/> holds necessary information for looking up & updating</param>
    /// <returns>Updated <see cref="Transaction"/></returns>
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

    /// <summary>Method deletes a single <see cref="Transaction"/>
    /// (<paramref name="accountId"/>, <paramref name="transactionId"/>)
    /// </summary>
    /// <param name="accountId"><c>AccountId</c> for looking up <see cref="Transaction"/></param>
    /// <param name="transactionId"><c>TransactionId</c> for looking up <see cref="Transaction"/></param>
    /// <returns>Deleted <see cref="Transaction"/></returns>
    public Transaction? DeleteTransaction(int accountId, int transactionId){
        var delete = GetTransaction(accountId, transactionId);

        if(delete is null)
            return null;
        
        _context.Transactions.Remove(delete);
        _context.SaveChanges();

        return delete;
    }

    public List<Transaction> DeleteAccountTransactions(int accountId){
        var delete = GetTransactions(accountId);

        foreach(Transaction del in delete){
            _context.Transactions.Remove(del);
        }

        if(delete.Count > 0)
            _context.SaveChanges();

        return delete;
    }
}