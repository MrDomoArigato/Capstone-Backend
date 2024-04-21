using System.Security.Claims;
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
    //ILogger<BudgetController> logger,
    IBudgetService budgetService
) : ControllerBase
{
    //private readonly CapstoneContext _context = context;
    //private readonly ILogger<BudgetController> _logger = logger;
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

    [HttpGet]
    public async Task<ActionResult<List<List<BudgetDTO>>>> GetBudget()
    {
        // Get user claims from the authenticated principal
        var claims = HttpContext.User.Claims;

        // Retrieve specific claim values
        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //var userId = "1";
        if(userId is null)
            return BadRequest();

        var result = await _bService.GetBudget(userId);
        if(result is null)
            return BadRequest();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateBudget(List<List<BudgetDTO>> budgets){
        // Get user claims from the authenticated principal
        var claims = HttpContext.User.Claims;

        // Retrieve specific claim values
        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //var userId = "6a1a7031350d84a9a7a39a125b42e2617abc01693f563dd4b1118f61776f5753";
        if(userId is null)
            return BadRequest();

        var created = _bService.CreateBudget(userId, budgets);
        if(created != 0)
            return BadRequest();

        var result = _bService.GetBudget(userId);
        if(result is null)
            return UnprocessableEntity();

        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateBudget(List<List<BudgetDTO>> budgets){
        // Get user claims from the authenticated principal
        var claims = HttpContext.User.Claims;

        // Retrieve specific claim values
        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if(userId is null)
            return BadRequest();

        var deleted = _bService.DeleteBudget(userId);
        if(deleted != 0)
            return BadRequest();
        
        var created = _bService.CreateBudget(userId, budgets);
        if(created != 0)
            return UnprocessableEntity();

        var result = _bService.GetBudget(userId);
        if(result is null)
            return UnprocessableEntity();
        
        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteBudget(){
        // Get user claims from the authenticated principal
        var claims = HttpContext.User.Claims;

        // Retrieve specific claim values
        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if(userId is null)
            return BadRequest();
        
        var result = _bService.DeleteBudget(userId);

        if(result != 0)
            return BadRequest();
        return Ok();
    }
}