using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CapstoneApi.Database;
using CapstoneApi.Data;
using Microsoft.VisualBasic;
using CapstoneApi.Services;

namespace CapstoneApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController(
    CapstoneContext context, 
    ILogger<TransactionController> logger, 
    ITransactionService transactionService) : ControllerBase
{
    private readonly CapstoneContext _context = context;
    private readonly ILogger<TransactionController> _logger = logger;

    private readonly ITransactionService _tService = transactionService;

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
        return Ok(await _tService.GetTransactionsAsync(accountId));
    }

    [HttpGet("{accountId}:{transactionId}")]
    public async Task<ActionResult<Transaction>> GetTransaction(int accountId, int transactionId)
    {
        var transaction = await _tService.GetTransactionAsync(accountId, transactionId);

        if(transaction is null)
            return BadRequest();
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction(Transaction rawTransaction){
        var result = _tService.CreateTransaction(rawTransaction);
        if(result is null)
            return BadRequest();
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<Transaction>> UpdateTransaction(Transaction updated){
        var result = await _tService.UpdateTransactionAsync(updated);

        if(result is null)
            return BadRequest();
        return result;
    }

    [HttpDelete("{accountId}:{transactionId}")]
    public async Task<ActionResult> DeleteTransaction(int accountId, int transactionId){
        var transaction = _tService.DeleteTransaction(accountId, transactionId);

        if(transaction is null)
            return BadRequest();

        return Ok();
    }

       [HttpGet("orderByDate")]
    public IActionResult GetOrderedByDate(int accountId, int transactionId)
    {
        var transactions = _tService.GetTransactionsOrderedByDate(accountId,transactionId);
        return Ok(transactions);
    }

    [HttpGet("orderById")]
    public IActionResult GetOrderedById(int accountId, int transactionId)
    {
        var transactions = _tService.GetTransactionsOrderedById(accountId,transactionId);
        return Ok(transactions);
    }
}
