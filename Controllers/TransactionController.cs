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

    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetTransactions()
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
}
