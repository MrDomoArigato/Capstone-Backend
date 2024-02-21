using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CapstoneApi.Database;
using CapstoneApi.Data;

namespace CapstoneApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController(CapstoneContext context, ILogger<TransactionController> logger) : ControllerBase
{
    private readonly CapstoneContext _context = context;
    private readonly ILogger<TransactionController> _logger = logger;

    //TODO Remove b4 release
    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
    {
        var transactions = await _context.Transactions.ToListAsync();
        /* new HashSet<Transaction>
        {
            new Transaction
            {
                TransactionId = 1,
                AccountId = 1,
                Amount = 1.00m,
                Balance = 2.00m,
                TransactionType = "None",
                Description = "Description",
                ModificationDate = DateTime.Now,
                CreationDate = DateTime.Now
            }
        }; */

        return Ok(transactions);
    }

    [HttpGet("{accountId}")]
    public async Task<ActionResult<List<Transaction>>> GetTransactions(int accountId)
    {
        var transactions = await _context.Transactions.Where(t => t.AccountId == accountId).ToListAsync();

        return Ok(transactions);
    }

    [HttpGet("{accountId}:{transactionId}")]
    public async Task<ActionResult<List<Transaction>>> GetTransaction(int accountId, int transactionId)
    {
        var transactions = await _context.Transactions
            .Where(t => t.AccountId == accountId)
            .Where(t => t.TransactionId == transactionId)
            .ToListAsync();

        return Ok(transactions);
    }

    [HttpPost("{accountId}:{transactionId}")]
    public void CreateTransaction(int accountId, int transactionId){
        
    }
}
