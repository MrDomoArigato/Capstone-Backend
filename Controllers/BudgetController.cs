using CapstoneApi.Data;
using CapstoneApi.Database;
using CapstoneApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneApi.Controllers;

//Controller
[ApiController]
[Route("[controller]")]
public class BudgetController(
    //CapstoneContext context, 
    ILogger<BudgetController> logger,
    IBudgetService budgetService
) : ControllerBase
{
    //private readonly CapstoneContext _context = context;
    private readonly ILogger<BudgetController> _logger = logger;
    private readonly IBudgetService _bService = budgetService;

    /* #warning TODO: Remove b4 release
    [HttpGet]
    public async Task<ActionResult<List<Budget>>> GetAllBudgets()
    {
        var budgets = _bService.GetAllBudgets();
        return Ok(budgets);
    } */

    /* #warning TODO: Remove b4 release
    [HttpPost]
    public async Task<ActionResult<List<Budget>>> TestCreateBudget(int userId, Dictionary<string, string> budget)
    {
        _bService.TestCreateBudget(new Budget{
            UserId = userId,
            BudgetItems = budget
        });
        return Ok();
    } */

    [HttpGet("{userId}")]
    public async Task<ActionResult<List<List<BudgetDTO>>>> GetBudget(int userId)
    {
        var result = await _bService.GetBudget(userId);
        if(result is null)
            return BadRequest();
        return Ok(result);
    }

    [HttpPost("{userId}")]
    public async Task<ActionResult> CreateBudget(int userId, List<List<BudgetDTO>> budgets){
        var result = _bService.CreateBudget(userId, budgets);

        if(result != 0)
            return BadRequest();
        return Ok();
    }
}