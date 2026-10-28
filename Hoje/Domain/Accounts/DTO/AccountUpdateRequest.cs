using Hoje.Domain.Accounts;

namespace Hoje.Domain.Accounts.DTO;
public class AccountUpdateRequest
{
    public string? Name { get; set; } 
    public string? Description { get; set; } 
    public string? Type { get; set; }
    public long? OpeningBalance { get; set; }
    public DateOnly? OpeningDate { get; set; }
    public string? Currency { get; set; }
    public string? Source { get; set; }
    public bool? IsActive { get; set; }

}