using CapstoneApi.Data;
using CapstoneApi.Database;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace CapstoneApi.Services;

public interface IBudgetService
{
    Task<List<List<BudgetDTO>>?> GetBudget(int userId);
    int? CreateBudget(int userId, List<List<BudgetDTO>> budgets);

    void TestCreateBudget(Budget budget);

    List<Budget> GetAllBudgets();
}

public class BudgetService(
    CapstoneContext context,
    ILogger<BudgetService> logger) : IBudgetService {
    private readonly CapstoneContext _context = context;
    private readonly ILogger<BudgetService> _logger = logger;

    public List<Budget> GetAllBudgets(){
        return _context.Budgets.ToList();
    }

    public async Task<List<List<BudgetDTO>>?> GetBudget(int userId){
        var buds = await _context.Budgets.FindAsync(userId);
        if (buds is null || buds.BudgetItems is null)
            return null;
        var transTypes = _context.TransactionTypes.Where(x => buds.BudgetItems.Keys.Contains(x.Id.ToString())).OrderBy(x => x.Id).ToList();
        var results = new List<List<BudgetDTO>>();
        var subs = new List<BudgetDTO>{};
        foreach(var type in transTypes)
        {
            if(type.Id % 1000 == 0){
                if(subs.Count > 0)
                    results.Add(subs);
                subs = [];
            } 
            subs.Add(new BudgetDTO{
                Id = type.Id,
                Code = type.Code,
                Description = type.Description,
                Amount = decimal.Parse(buds.BudgetItems[type.Id.ToString()])
            });
        }
        results.Add(subs);
        return results;
    }

    
    public int? CreateBudget(int userId, List<List<BudgetDTO>> budgets){
        var transTypes = _context.TransactionTypes;
        var budMap = new Dictionary<string, string>();
        BudgetDTO last = null;
        decimal total = 0;
        foreach(var bud in budgets.SelectMany(b => b).OrderBy(b => b.Id)){
            if(bud.Id % 1000 != 0 && bud.Amount > 0){
                budMap.Add(bud.Id.ToString(), bud.Amount.ToString());
                total += bud.Amount;
            }
            if(last is not null && (int)(bud.Id / 1000) > (int)(last.Id / 1000)){
                budMap.Add(((int)(bud.Id / 1000) * 1000).ToString(), total.ToString());
                total = 0;
            }
            last = bud;
        }
        if(last is not null)
            budMap.Add(((int)(last.Id / 1000) * 1000).ToString(), total.ToString());
        _context.Budgets.Add(new Budget{UserId = userId, BudgetItems = budMap});
        _context.SaveChanges();
        // var budgetDb = _context.Budgets.Find(userId);
        // if(budgetDb is null)
        //     return 1;
        // budgetDb.BudgetItems = budMap;
        // _context.Budgets.Update(budgetDb);
        // _context.SaveChanges();
        return 0;
    } 

    public void TestCreateBudget(Budget budget){
        _context.Budgets.Add(budget);
        _context.SaveChanges();
    }
}