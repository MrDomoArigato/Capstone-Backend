using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CapstoneApi.Database;
using CapstoneApi.Data;
using CapstoneApi.Services;

namespace CapstoneApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController(
    //CapstoneContext context, 
    //ILogger<TransactionController> logger, 
    IAccountService accountService,
    ITransactionService transactionService) : ControllerBase
{
    //private readonly CapstoneContext _context = context;
    //private readonly ILogger<TransactionController> _logger = logger;
    private readonly IAccountService _aService = accountService;
    private readonly ITransactionService _tService = transactionService;

    /* #warning TODO: Remove b4 release
    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
    {
        var transactions = await _context.Transactions.ToListAsync();
        return Ok(transactions);
    } */

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
        return Ok(transaction);
    }

    [HttpGet("TransactionTypes")]
    public async Task<ActionResult<List<List<TransactionType>>>> GetTransactionTypes()
    {
        var results = _tService.GetTransactionTypes();

        return Ok(results);
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction(Transaction rawTransaction){
        if(_aService.GetAccount(rawTransaction.AccountId) is null)
            return BadRequest();
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
}
