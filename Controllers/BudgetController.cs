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
    IBudgetService budgetService
) : ControllerBase
{
    private readonly IBudgetService _bService = budgetService;

    [HttpGet]
    public async Task<ActionResult<List<List<BudgetDTO>>>> GetBudget()
    {
        // Get user claims from the authenticated principal
        var claims = HttpContext.User.Claims;

        // Retrieve specific claim values
        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
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