using System;
using System.Collections.Generic;

namespace CapstoneApi.Data;

public partial class BudgetDTO
{
    public int Id { get; init; }
    public string? Code { get; init; }
    public string? Description { get; init; }
    public decimal Amount { get; set; }
}

public partial class Budget
{
    public required string UserId { get; init; }
    public Dictionary<string, string>? BudgetItems { get; set; }
}
