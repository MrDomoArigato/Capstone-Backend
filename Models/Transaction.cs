using System;
using System.Collections.Generic;

namespace CapstoneApi.Data;

public partial class Transaction
{
    public int TransactionId { get; init; }

    public int AccountId { get; init; }

    public decimal Amount { get; set; }

    public string? TransactionType { get; set; }

    public string? Description { get; set; }

    public DateOnly? TransactionDate{ get; set; }

    public DateTime? ModificationDate { get; set; }

    public DateTime CreationDate { get; init; }
}
