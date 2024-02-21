

namespace CapstoneApi.Data;

//Account Model
public class Account
{
    public int AccountId { get; set; } 
    public required string AccountName { get; set; } 
    public required string AccountOwner { get; set; } 
    public decimal Balance { get; set; }
    public required ICollection<Transaction> Transactions { get; set; } 
}
