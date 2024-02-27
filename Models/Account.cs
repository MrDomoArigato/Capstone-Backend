

namespace CapstoneApi.Data;

//Account Model
public class Account
{
    public int AccountId { get; init; } 

    public string? AccountName { get; set; } 

    public string? AccountOwner { get; set; }

    public decimal? Balance { get; set; }

    public DateTime? BalanceModification { get; set; }

    public DateTime? ModificationDate { get; set; }

    public DateTime? CreationDate { get; init; }
}
