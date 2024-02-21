using System.Collections.Generic;
using CapstoneApi.Data;
using CapstoneApi.Database;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneApi.Controllers;

//Account Controller
[ApiController]
[Route("[controller]")]
public class AccountController(CapstoneContext context, ILogger<AccountController> logger) : ControllerBase
{
    private readonly CapstoneContext _context;

    private readonly ILogger<AccountController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<List<Account>>> GetAllAccounts()
    {
        var account = new List<Account>{
            new Account{
                AccountId = 1,
                AccountName = "Account Name",
                AccountOwner = "owner",
                Balance = 1.00m
            }
        };
        return Ok(account);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        var account = new List<Account>{
            new Account{
                AccountId = 1,
                AccountName = "Account Name",
                AccountOwner = "owner",
                Balance = 1.00m
            }
        };
        return Ok(account);
    }
}

