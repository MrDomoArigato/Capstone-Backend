using CapstoneApi.Data;
using CapstoneApi.Database;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace CapstoneApi.Services;

public interface IBudgetService
{
    Task<List<List<BudgetDTO>>?> GetBudget(string userId);
    int? CreateBudget(string userId, List<List<BudgetDTO>> budgets);

    int? DeleteBudget(string userId);

    int? UpdateBudgetItem(string userId, BudgetDTO budget);

    int? DeleteBudgetItem(string userId, int budgetId);
}

public class BudgetService(
    CapstoneContext context
    ) : IBudgetService {
    private readonly CapstoneContext _context = context;

    public async Task<List<List<BudgetDTO>>?> GetBudget(string userId){
        var results = new List<List<BudgetDTO>>();
        var buds = await _context.Budgets.FindAsync(userId);
        if (buds is null || buds.BudgetItems is null)
            return null;
        var transTypes = _context.TransactionTypes.Where(x => buds.BudgetItems.Keys.Contains(x.Id)).OrderBy(x => x.Id).ToList();
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
                Amount = buds.BudgetItems[type.Id]
            });
        }
        results.Add(subs); 
        return results;
    }

    
    public int? CreateBudget(string userId, List<List<BudgetDTO>> budgets){
        var budMap = new Dictionary<int, decimal>();
        var budList = budgets.SelectMany(b => b).OrderBy(b => b.Id).ToList();
        var total = 0m;
        for(var idx = 0; idx < budList.Count; idx++)
        {
            if(budList[idx].Id % 1000 != 0){
                budMap.Add(budList[idx].Id, budList[idx].Amount);
                total += budList[idx].Amount;
            }
            if(idx + 1 == budList.Count || (budList[idx].Id / 1000) < (budList[idx + 1].Id / 1000)){
                budMap.Add(((int)(budList[idx].Id / 1000) * 1000), total);
                total = 0;
            }
        }
        _context.Budgets.Add(new Budget{UserId = userId, BudgetItems = budMap});
        _context.SaveChanges();
        return 0;
    }

    public int? DeleteBudget(string userId){
        Budget delete = _context.Budgets.Find(userId)!;
        if(delete is null)
            return 1;

        _context.Budgets.Remove(delete);
        _context.SaveChanges();
        return 0;
    }

    public int? UpdateBudgetItem(string userId, BudgetDTO budget){
        Budget update = _context.Budgets.Find(userId)!;
        update ??= _context.Budgets.Add(new Budget{UserId = userId, BudgetItems = new Dictionary<int, decimal>()}).Entity;

        update.BudgetItems![budget.Id] = budget.Amount;
        _context.Entry(update).Property(u => u.BudgetItems).IsModified = true;

        _context.SaveChanges();
        return 0;
    }

    public int? DeleteBudgetItem(string userId, int budgetId){
        Budget delete = _context.Budgets.Find(userId)!;
        if(delete is null)
            return 1;

        delete.BudgetItems!.Remove(budgetId);
        _context.Entry(delete).Property(u => u.BudgetItems).IsModified = true;

        _context.SaveChanges();
        return 0;
    }
} 