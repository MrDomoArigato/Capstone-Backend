using CapstoneApi.Data;
using CapstoneApi.Database;
using CapstoneApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneApi.Controllers;

//Account Controller
[ApiController]
[Route("[controller]")]
public class AccountController(
    ILogger<AccountController> logger, 
    IAccountService accountService,
    ITransactionService transactionService) : ControllerBase
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly IAccountService _aService = accountService;
    private readonly ITransactionService _tService = transactionService;

    [HttpGet("test")]
    public async Task<ActionResult<List<Account>>> GetAllAccounts()
    {
        var accounts = await _aService.GetAllAccounts();

        if(accounts is null)
            return BadRequest();
        return Ok(accounts);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        var account = _aService.GetAccount(id);

        if(account is null)
            return BadRequest();
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAccount(int id){
        var account = _aService.DeleteAccount(id);
        _tService.DeleteAccountTransactions(id);

        if(account is null)
            return BadRequest();   

        return Ok();
    }
}

