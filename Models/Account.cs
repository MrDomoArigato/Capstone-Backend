

namespace CapstoneApi.Data;

//Account Model
public class Account
{
    public int AccountId { get; set; } 
    public string? AccountName { get; set; } 
    public string? AccountOwner { get; set; } 
    public decimal? Balance { get; set; }
}
