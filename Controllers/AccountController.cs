using System.Security.Claims;
using CapstoneApi.Data;
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
    

    [HttpGet("user")]
    public async Task<ActionResult<List<Account>>> GetUserAccounts()
    {
        // Get user claims from the authenticated principal
        var claims = HttpContext.User.Claims;

        // Retrieve specific claim values
        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var accounts = _aService.GetUserAccounts(userId);

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
        // Get user claims from the authenticated principal
        var claims = HttpContext.User.Claims;
        // Retrieve specific claim values
        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        newAccount.AccountOwner = userId;
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

