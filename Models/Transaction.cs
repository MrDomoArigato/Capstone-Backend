﻿using System;
using System.Collections.Generic;

namespace CapstoneApi.Data;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int AccountId { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Balance { get; set; }

    public string? TransactionType { get; set; }

    public string? Description { get; set; }

    public DateTime? ModificationDate { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? Type { get; set; }
}
