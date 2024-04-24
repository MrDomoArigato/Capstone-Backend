using CapstoneApi.Data;
using CapstoneApi.Database;
using CapstoneApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneApi.Controllers;

//Account Controller
[ApiController]
[Route("[controller]")]
public class AccountController(
    CapstoneContext context, 
    ILogger<AccountController> logger, 
    IAccountService accountService) : ControllerBase
{
    private readonly CapstoneContext _context = context;

    private readonly ILogger<AccountController> _logger = logger;

    private readonly IAccountService _aService = accountService;

    [HttpGet]
    public async Task<ActionResult<List<Account>>> GetAllAccounts()
    {
        return Ok(await _context.Accounts.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        var account = new List<Account>{
            new() {
                AccountId = 1,
                AccountName = "Account Name",
                AccountOwner = "owner",
                Balance = 1.00m
            }
        };
        return Ok(account);
    }

    [HttpPost]
    public async Task<ActionResult<Account>> CreateAccount(Account newAccount){
        var result = _aService.CreateAccount(newAccount);
        if(result is null)
            return BadRequest();
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<Account>> UpdateAccount(Account updated){
        var result = _aService.UpdateAccount(updated);

        if(result is null)
            return BadRequest();
        return result;
    }

    [HttpDelete("{accountId}:{transactionId}")]
    public async Task<ActionResult> DeleteTransaction(int accountId, int transactionId){
        var transaction = await _context.Transactions.FindAsync(new {transactionId, accountId});

        if(transaction is null)
            return BadRequest();

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();     

        return Ok();
    }

    
    [HttpGet]
    public async Task<ActionResult<list<Account>>> GetAccounts([FromQuery] int page = 1, [FromQuery] int count = 100){
        var accounts = await _context.Accounts
                                .Skip((page - 1) * count)
                                .Take(count)
                                .ToListAsync();

        return Ok(accounts);
    }

    // This method should be part of the IAccountService implementation
public async Task<Account?> UpdateBalanceAsync(int accountId, decimal newBalance)
{
    var current = await GetAccountAsync(accountId);

    if (current == null)
        return null;

    current.Balance = newBalance;
    current.BalanceModification = DateTime.Now;

    try {
        await _context.SaveChangesAsync();
        return current;
    } catch (Exception ex) {
        _logger.LogError(ex, "Error updating balance for account {AccountId}", accountId);
        // Optionally, handle specific types of exceptions if needed
        return null;
    }
}

}

