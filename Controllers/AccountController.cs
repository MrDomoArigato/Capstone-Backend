using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace capstone.Controllers;



//Updated DbContext
public ISet<Account> Accounts { get; set; }

//Account Controller
[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AccountsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
    {
        return await _context.Accounts.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        var account = await _context.Accounts
            .Include(a => a.Transactions) 
            .FirstOrDefaultAsync(a => a.AccountId == id);

        if (account == null)
        {
            return NotFound();
        }

        return account;
    }
}

